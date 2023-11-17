using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataAccess.Abstract;
using Domain.Entity;
using Domain.Utility;
using OnBarcode.Barcode;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Domain.DataAccess.Concrete
{
    public class ProduceSessionRepository : IProduceSessionRepository
    {
        private readonly EWCEntities _dbContext;

        public ProduceSessionRepository()
        {
            _dbContext = new EWCEntities();
        }

        public void Dispose() {
            if (_dbContext != null) {
                _dbContext.Dispose();
            }
        }

        public List<DishItem> GetDishItemByDate(DateTime date)
        {
            List<DishItem> listDishItem = new List<DishItem>();
            try
            {
                listDishItem = _dbContext.DishItems.Where(di => di.CookingTime.Value.Day == date.Day && di.CookingTime.Value.Month == date.Month && di.CookingTime.Value.Year == date.Year && di.IsAvailable == true).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return listDishItem;
        }

        // get menu of a day
        public Menu GetMenuByDate(DateTime date)
        {
            try
            {
                var menu = _dbContext.Menus.Where(m => m.IsAvailable && m.ApplyDate.Equals(date.Date)).FirstOrDefault();
                return menu;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        //Load dish from menu by menuID.
        public List<Dish> LoadDishFromMenu(int menuID)
        {
            try
            {
                var ListDishMenu = _dbContext.DishMenus.Where(d => d.MenuID == menuID).ToList();
                List<Dish> ListDish = null;
                if (ListDishMenu != null)
                {
                    List<int> ids = new List<int>();
                    foreach (var item in ListDishMenu)
                    {
                        ids.Add(item.DishID);
                    }
                    ListDish = _dbContext.Dishes.Where(d => ids.Contains(d.Id) && d.IsAvailable == true).ToList();
                    return ListDish;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DishItem GetDishItemById(int dishItemId)
        {
            DishItem dishItem = new DishItem();
            try
            {
                // eager loading Dish
                dishItem = _dbContext.DishItems.Where(d => d.Id == dishItemId).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
            return dishItem;
        }

        // calculate number of produced DishItem
        public int CalculateNumberProducedDishItemOfMenu(Menu menu, int dishID)
        {
            int numberOfProducedDishItem = 0;
            List<DishItem> listDishItem = new List<DishItem>();
            try
            {
                var today = DateTime.Today;
                listDishItem = _dbContext.DishItems.Where(di => di.CookingTime.Value.Day == menu.ApplyDate.Day && di.CookingTime.Value.Month == menu.ApplyDate.Month && di.CookingTime.Value.Year == menu.ApplyDate.Year && di.DishID == dishID && di.IsAvailable).ToList();
                foreach (DishItem dishItem in listDishItem)
                {
                    numberOfProducedDishItem += dishItem.Quantity;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return numberOfProducedDishItem;
        }

        // calculate number of ordered DishItem
        public int CalculateNumberOrderedDishItem(Menu menu, int dishID)
        {
            int numberOfOrderedDishItem = 0;
            List<OrderDetail> listOrderDetail = new List<OrderDetail>();
            try
            {
                listOrderDetail = _dbContext.OrderDetails.Where(od => od.DishID == dishID && od.Order.OrderStatus != (int)OrderStatusEnum.Cancel && od.Order.DeliveryDate.Day == menu.ApplyDate.Day && od.Order.DeliveryDate.Month == menu.ApplyDate.Month && od.Order.DeliveryDate.Year == menu.ApplyDate.Year).ToList();
                foreach (OrderDetail orderDetail in listOrderDetail)
                {
                    numberOfOrderedDishItem += orderDetail.Quantity;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            return numberOfOrderedDishItem;
        }

        // get all IngredientItem available for a Dish
        public List<IngredientItem> GetAvailableIngredientItemForDish(int dishId)
        {
            List<IngredientItem> listIngredientItem = new List<IngredientItem>();
            try
            {
                listIngredientItem = _dbContext.IngredientItems.Where(ii => ii.Ingredient.Dishes.Any(d => d.Id == dishId) && ii.IsAvailable && ii.Ingredient.IsTracibility).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return listIngredientItem;
        }

        // produce DishItem 
        public DishItem ProduceDishItem(int dishID, int quantity, string ingredients_str)
        {
            try
            {
                Dish dish = new Dish();
                dish = _dbContext.Dishes.Where(d => d.Id == dishID).FirstOrDefault();
                if (dish == null) return null;

                string[] ingredients_arr = ingredients_str.Split(',');
                DishItem dishItem = new DishItem();
                dishItem.Dish = dish;
                dishItem.DishID = dishID;
                dishItem.CookingTime = DateTime.Now;
                dishItem.Quantity = quantity;
                dishItem.IsAvailable = true;
                dishItem.ChefID = 1;

                foreach (string ingredient_str in ingredients_arr)
                {
                    int ingredient_id = 0;
                    if (Int32.TryParse(ingredient_str, out ingredient_id))
                    {
                        IngredientItem ingredientItem = new IngredientItem();

                        ingredientItem = _dbContext.IngredientItems.Where(ii => ii.Id == ingredient_id).FirstOrDefault();
                        if (ingredientItem == null) return null;
                        dishItem.IngredientItems.Add(ingredientItem);
                    }
                    else
                    {
                        return null;
                    }
                }
                
                _dbContext.DishItems.Add(dishItem);
                _dbContext.SaveChanges();
                return dishItem;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool ProduceSessionGenerate(List<ProduceSessionGenerateDTO> ListProduceSessionGenerateDTO, string serverPath)
        {
            List<DishItem> ListDishItemTemp = new List<DishItem>();
            // create list dish item
            foreach (ProduceSessionGenerateDTO produceSessionDTO in ListProduceSessionGenerateDTO)
            {
                Dish dish = new Dish();
                dish = _dbContext.Dishes.Where(d => d.Id == produceSessionDTO.DishID).FirstOrDefault();
                if (dish == null) return false;

                string[] ingredients_arr = produceSessionDTO.IngredientsString.Split(',');
                DishItem dishItem = new DishItem();
                dishItem.Dish = dish;
                dishItem.DishID = produceSessionDTO.DishID;
                dishItem.CookingTime = DateTime.Now;
                dishItem.Quantity = produceSessionDTO.Quantity;
                dishItem.IsAvailable = true;
                dishItem.ChefID = 1;
                foreach (string ingredient_str in ingredients_arr)
                {
                    int ingredient_id = 0;
                    if (Int32.TryParse(ingredient_str, out ingredient_id))
                    {
                        IngredientItem ingredientItem = new IngredientItem();

                        ingredientItem = _dbContext.IngredientItems.Where(ii => ii.Id == ingredient_id).FirstOrDefault();
                        if (ingredientItem == null) return false;
                        dishItem.IngredientItems.Add(ingredientItem);
                    }
                    else
                    {
                        return false;
                    }
                }
                _dbContext.DishItems.Add(dishItem);
                ListDishItemTemp.Add(dishItem);
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            // create qrcode
            bool result = false;
            foreach (DishItem dishItem in ListDishItemTemp)
            {
                String guid = System.Guid.NewGuid().ToString();
                string path = Path.Combine(serverPath, guid);
                result = this.CreateQRCodeForDishItem(dishItem, path);

                if (!result)
                {
                    result = false;
                    break;
                }
            }

            if (!result)
            {
                foreach (DishItem dishItem in ListDishItemTemp)
                {
                    _dbContext.DishItems.Remove(dishItem);
                    _dbContext.SaveChanges();
                }
                return false;
            }
            return true;
        }

        public bool CreateQRCodeForDishItem(DishItem dishItem, string path)
        {
            string gifExtension = ".gif";
            bool saveFile = false;
            string filename = path.Substring(path.LastIndexOf('\\') + 1);
            try
            {
                // generate qrcode 
                QRCode qrcode = new QRCode();

                // Barcode data to encode
                qrcode.Data = dishItem.Id.ToString();
                // QR-Code data mode
                qrcode.DataMode = QRCodeDataMode.AlphaNumeric;
                // QR-Code format mode
                //qrcode.Version = QRCodeVersion.V10;

                /*
                * Barcode Image Related Settings
                */
                // Unit of meature for all size related setting in the library. 
                qrcode.UOM = UnitOfMeasure.PIXEL;
                // Bar module size (X), default is 3 pixel;
                qrcode.X = 3;
                // Barcode image left, right, top, bottom margins. Defaults are 0.
                qrcode.LeftMargin = 0;
                qrcode.RightMargin = 0;
                qrcode.TopMargin = 0;
                qrcode.BottomMargin = 0;
                // Image resolution in dpi, default is 72 dpi.
                qrcode.Resolution = 72;
                // Created barcode orientation.
                //4 options are: facing left, facing right, facing bottom, and facing top
                qrcode.Rotate = Rotate.Rotate0;

                // Generate QR-Code and encode barcode to gif format
                qrcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Gif;
                qrcode.drawBarcode(path + "_temp" + gifExtension);

                // drawing information in top of image
                Bitmap tempbmp = new Bitmap(path + "_temp" + gifExtension);
                Bitmap bmp = new Bitmap(tempbmp.Width + 137, tempbmp.Height + 237);
                Graphics graphics = Graphics.FromImage(bmp);
                graphics.DrawImage(tempbmp, new Rectangle(0, 100, bmp.Width, bmp.Height - 100));
                RectangleF rectf = new Rectangle(0, 0, bmp.Width, 100);

                graphics.SmoothingMode = SmoothingMode.Default;
                graphics.InterpolationMode = InterpolationMode.Default;
                graphics.PixelOffsetMode = PixelOffsetMode.Default;
                graphics.DrawString(dishItem.Dish.Name + " - MS: " + dishItem.Id + " - " + dishItem.CookingTime, new Font("Tahoma", 15), Brushes.White, rectf);
                
                bmp.Save(path + gifExtension, ImageFormat.Gif);
                graphics.Flush();
                bmp.Dispose();
                tempbmp.Dispose();
                File.Delete(path + "_temp" + gifExtension);
               
                // update qrcode for dish item
                saveFile = true;
                dishItem.QRCode = filename + gifExtension;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                // delete file image if exception
                if (saveFile == true)
                {
                    FileHelper.DeleteFileFromSystem(path + gifExtension);
                }
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool DeleteDishItem(int dishItemId)
        {
            try
            {
                DishItem dishItem = new DishItem();
                dishItem = _dbContext.DishItems.Where(di => di.Id == dishItemId && di.IsAvailable == true).FirstOrDefault();
                if (dishItem == null || dishItem.OrderDetailDishItems.Count != 0) return false;
                dishItem.IsAvailable = false;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool DeleteDishItemPermanent(DishItem dishItem)
        {
            try
            {
                _dbContext.DishItems.Remove(dishItem);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
