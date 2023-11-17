using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ElFinder;

namespace EatWithChef.Areas.Admin.Controllers
{
    public partial class AdminController : Controller
    {
        private Connector _connector;

        public Connector Connector
        {
            get
            {
                if (_connector == null)
                {
                    FileSystemDriver driver = new FileSystemDriver();
                    DirectoryInfo thumbsStorage = new DirectoryInfo(Server.MapPath("~/Images"));
                    //driver.AddRoot(new Root(new DirectoryInfo(@"D:\Images"))
                    //{
                    //    IsLocked = true,
                    //    IsReadOnly = true,
                    //    IsShowOnly = true,
                    //    ThumbnailsStorage = thumbsStorage,
                    //    ThumbnailsUrl = "Thumbnails/"
                    //});
                    driver.AddRoot(new Root(new DirectoryInfo(Server.MapPath("~/Images")), "/Images/")
                    {
                        //Alias = "Thư viện ảnh",
                        Alias = "/Images",
                        StartPath = new DirectoryInfo(Server.MapPath("~/Images")),
                        ThumbnailsStorage = thumbsStorage,
                        MaxUploadSizeInMb = 2.2,
                        ThumbnailsUrl = "Thumbnails/",

                    });
                    _connector = new Connector(driver);
                }
                return _connector;
            }
        }
        /// <summary>
        /// Load folder to connector
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadFile()
        {
            return Connector.Process(this.HttpContext.Request);
        }
        public ActionResult SelectFile(List<String> values)
        {
            var returnlist = "";
            foreach (var file in values)
            {
                var sliceString = Connector.GetFileByHash(file).FullName.Split(new string[] { @"\Images\" }, StringSplitOptions.None);
                //string[] sliceString = Regex.Split(Connector.GetFileByHash(file).FullName, @"\VC\");
                var url = sliceString[1].Replace(@"\", "/").Replace(@"\\", "/");
                returnlist += "/Images/" + url + ";";
            }
            return Json(returnlist);
        }

        public ActionResult Thumbs(string tmb)
        {
            return Connector.GetThumbnail(Request, Response, tmb);
        }

        //public ActionResult Elfinder()
        //{
        //    return View("_Elfinder");
        //}
        public ActionResult GetImageFromElfinder()
        {
            return View("_Elfinder");
        }
    }
}
