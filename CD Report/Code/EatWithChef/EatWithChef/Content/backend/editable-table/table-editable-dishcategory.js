var EditableTable = function () {

    return {

        //main function to initiate the module
        init: function () {
            function restoreRow(oTable, nRow) {
                var aData = oTable.fnGetData(nRow);
                var jqTds = $('>td', nRow);

                for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
                    oTable.fnUpdate(aData[i], nRow, i, false);
                }

                oTable.fnDraw();
                $("img.small-thumb").thumbPopup({
                    imgSmallFlag: "",
                    imgLargeFlag: "",
                    cursorTopOffset: 10,
                    cursorLeftOffset: 10
                });
            }

            function editRow(oTable, nRow, isNew) {
                var aData = oTable.fnGetData(nRow);
                var jqTds = $('>td', nRow);
                // create name
                jqTds[0].innerHTML = '<input type="text" id="name" name="name" maxlength="100" style="width:150px" class="form-control small" value="' + aData[0] + '">';
                jqTds[0].innerHTML += '<p class="alert-danger" style="margin-top:7px; visibility:hidden">Tên này đã tồn tại!</p>';
                // create description
                jqTds[1].innerHTML = '<textarea class="form-control" id="description" name="description" rows="3">' + aData[1] + '</textarea>';
                // create image
                var filepath = aData[2].split('"')[3];
                if (filepath != "" && filepath != null) {
                    var index = filepath.lastIndexOf('/');
                    var filename = filepath.substring(index + 1, filepath.length);
                    jqTds[2].innerHTML = '<div class="controls col-md-9">' +
                                            '<div class="fileupload fileupload-exists" data-provides="fileupload"><input type="hidden" value="" name="">' +
                                                '<span class="btn btn-white btn-file" onclick="browse_image(this)">' +
                                                '<span class="fileupload-new"><i class="icon-camera-retro"></i> Chọn hình</span>' +
                                                '<span class="fileupload-exists"><i class="icon-camera-retro"></i> Chọn hình</span>' +
                                                '<input type="hidden" id="image" name="image" value="' + filepath  + '">' +
                                                '</span>' +
                                                '<span class="fileupload-preview" style="margin-left:5px;">' + filename + '</span>' +
                                                '<a href="#" class="close fileupload-exists" data-dismiss="fileupload" style="float: none; margin-left:5px;"></a>' +
                                            '</div>' +
                                        '</div>';
                } else {
                    jqTds[2].innerHTML = '<div class="controls col-md-9">' +
                                            '<div class="fileupload fileupload-exists" data-provides="fileupload"><input type="hidden" value="" name="">' +
                                                '<span class="btn btn-white btn-file" onclick="browse_image(this)">' +
                                                '<span class="fileupload-new"><i class="icon-camera-retro"></i> Chọn hình</span>' +
                                                '<span class="fileupload-exists"><i class="icon-camera-retro"></i> Chọn hình</span>' +
                                                '<input type="hidden" id="image" name="image" value="' + filepath + '">' +
                                                '</span>' +
                                                '<span class="fileupload-preview" style="margin-left:5px;"></span>' +
                                                '<a href="#" class="close fileupload-exists" data-dismiss="fileupload" style="float: none; margin-left:5px;"></a>' +
                                            '</div>' +
                                        '</div>';
                }

                // create isactive
                //if (aData[3] == "True") {
                //    jqTds[3].innerHTML = '<input type="checkbox" name="isactive" checked data-on="primary" data-off="info">';
                //} else {
                //    jqTds[3].innerHTML = '<input type="checkbox" name="isactive" data-on="primary" data-off="info">';
                //}
                //$('input[type="checkbox"],[type="radio"]').not('#create-switch').bootstrapSwitch();
                //$(jqTds[3].children[0]).find("input").attr("id", "create-switch");

                

                // create 2 button
                if (isNew) {
                    jqTds[3].innerHTML = '<center><button type="button" class="btn btn-danger cancel" data-mode="new"><i class="icon-remove"></i> Hủy</button></center>';
                    jqTds[4].innerHTML = '<center><button type="button" class="btn btn-info edit"><i class="icon-check"></i> Lưu</button></center>';
                } else {
                    jqTds[3].innerHTML = '<center><button type="button" class="btn btn-danger cancel"><i class="icon-remove"></i> Hủy</button></center>';
                    jqTds[4].innerHTML = '<center><button type="button" class="btn btn-info edit"><i class="icon-check"></i> Lưu</button></center>';
                    // validate category name
                    validateType($('input[name="name"]', nRow)[0], $('input[name="dish_category_id"]', nRow).val());
                }

                $('input[name="name"]', nRow).blur(function () {
                    validateType(this, $('input[name="dish_category_id"]', nRow).val());
                });
            }

            function saveRow(oTable, nRow, image) {
                oTable.fnUpdate($('#name', nRow)[0].value, nRow, 0, false);
                oTable.fnUpdate($('#description', nRow)[0].value, nRow, 1, false);
                oTable.fnUpdate('<center><img class="small-thumb" src="' + image + '" width="40px" height="40px"/></center>', nRow, 2, false);
                //if ($('#create-switch', nRow)[0].checked) {
                //    oTable.fnUpdate("Đã kích hoạt", nRow, 3, false);
                //} else {
                //    oTable.fnUpdate("Không kích hoạt", nRow, 3, false);
                //}
                // 2 button
                oTable.fnUpdate('<center><button type="button" class="btn btn-danger delete"><i class="icon-remove"></i> Xóa</button></center>', nRow, 3, false);
                oTable.fnUpdate('<center><button type="button" class="btn btn-info edit"><i class="icon-refresh"></i> Sửa</button></center>', nRow, 4, false);
                oTable.fnDraw();
                $("img.small-thumb").thumbPopup({
                    imgSmallFlag: "",
                    imgLargeFlag: "",
                    cursorTopOffset: 10,
                    cursorLeftOffset: 10
                });
            }

            function cancelEditRow(oTable, nRow, image) {
                oTable.fnUpdate($('#name', nRow)[0].value, nRow, 0, false);
                oTable.fnUpdate($('#description', nRow)[0].value, nRow, 1, false);
                oTable.fnUpdate('<center><img class="small-thumb" src="' + imageName + '" width="40px" height="40px"/></center>', nRow, 2, false);
                //if ($('#create-switch', nRow)[0].checked) {
                //    oTable.fnUpdate("Đã kích hoạt", nRow, 3, false);
                //} else {
                //    oTable.fnUpdate("Không kích hoạt", nRow, 3, false);
                //}
                oTable.fnUpdate('<center><button type="button" class="btn btn-danger delete"><i class="icon-remove"></i> Xóa</button></center>', nRow, 3, false);
                oTable.fnUpdate('<center><button type="button" class="btn btn-info edit"><i class="icon-refresh"></i> Sửa</button></center>', nRow, 4, false);
                oTable.fnDraw();
            }

            var oTable = $('#editable-sample').dataTable({
                "aLengthMenu": [
                    [5, 15, 20, -1],
                    [5, 15, 20, "All"] // change per page values here
                ],
                // set the initial value
                "iDisplayLength": 5,
                "sDom": "<'row'<'col-lg-6'l><'col-lg-6'f>r>t<'row'<'col-lg-6'i><'col-lg-6'p>>",
                "sPaginationType": "bootstrap",
                "oLanguage": {
                    "sLengthMenu": "_MENU_ records mỗi trang",
                    "oPaginate": {
                        "sPrevious": "Prev",
                        "sNext": "Next"
                    }
                }
            });

            jQuery('#editable-sample_wrapper .dataTables_filter input').addClass("form-control medium"); // modify table search input
            jQuery('#editable-sample_wrapper .dataTables_length select').addClass("form-control xsmall"); // modify table per page dropdown

            var nEditing = null;

            $('#editable-sample_new').click(function (e) {
                e.preventDefault();
                var aiNew = oTable.fnAddData(['', '', '', '', '']);
                var nRow = oTable.fnGetNodes(aiNew[0]);
                editRow(oTable, nRow, true);
                nEditing = nRow;

                var editing_tr = $(this).parent().parent().next('div').find('tr:has(button[class="btn btn-danger cancel"])')[1];
                if (editing_tr) {
                    // check is a other new or old row
                    if ($(editing_tr).find('button[data-mode="new"]')[0]) {
                        oTable.fnDeleteRow(editing_tr);
                    } else {
                        restoreRow(oTable, editing_tr);
                    }
                }

            });

            $('#editable-sample button.delete').live('click', function (e) {
                var nRow = $(this).parents('tr')[0];
                var id = $('input[name="dish_category_id"]', nRow).val();

                e.preventDefault();
                alertify.confirm("Bạn có thật sự muốn xóa Danh Mục này? Tất cả những Món Ăn trong Danh Mục cũng bị xóa!", function (e) {
                    if (e) {
                        $.ajax({
                            url: "/DishCategoryManagement/DeleteDishCategoryName?categoryid=" + id,  //Server script to process data
                            type: 'POST',
                            // Form data
                            cache: false,
                            contentType: false,
                            processData: false,
                            async: false,
                            success: function (data) {
                                if (data == "Success") {
                                    oTable.fnDeleteRow(nRow);
                                    hm.wHumanMsg('Xóa thành công.', { theme: 'green' });
                                } else {
                                    hm.wHumanMsg('Xóa thất bại.', { theme: 'red' });
                                }
                            },
                            error: function (errMsg) {
                                hm.wHumanMsg('Lỗi xảy ra. Xin thử lại sau.');
                            }
                        });
                    } else {
                        return;
                    }
                });

                
            });

            $('#editable-sample button.cancel').live('click', function (e) {
                e.preventDefault();
                if ($(this).attr("data-mode") == "new") {
                    var nRow = $(this).parents('tr')[0];
                    oTable.fnDeleteRow(nRow);
                    nEditing = null;
                } else {
                    restoreRow(oTable, nEditing);
                    nEditing = null;
                }
            });

            $('#editable-sample button.edit').live('click', function (e) {
                var new_tr = $(this).closest('tr').prevAll('tr:has(button[data-mode="new"])')[0];
                if (new_tr) {
                    oTable.fnDeleteRow(new_tr);
                    nEditing = null;
                }

                e.preventDefault();

                /* Get the row as a parent of the link that was clicked on */
                var nRow = $(this).parents('tr')[0];

                if (nEditing !== null && nEditing != nRow) {
                    /* Currently editing - but not this row - restore the old before continuing to edit mode */
                    restoreRow(oTable, nEditing);
                    editRow(oTable, nRow, false);
                    nEditing = nRow;
                } else if (nEditing == nRow && this.innerHTML == '<i class="icon-check"></i> Lưu') {
                    /* Editing this row and want to save it */
                    var formData = new FormData();
                    var url = '';
                    if ($('#dish_category_id', nRow).length != 0) {
                        url = '/DishCategoryManagement/UpdateDishCategory';
                        formData.append("id", $('#dish_category_id', nRow)[0].value);

                        var check = validateType($('input[name="name"]', nRow)[0], $('#dish_category_id', nRow)[0].value);
                        if (!check) {
                            return;
                        }
                    } else {
                        url = '/DishCategoryManagement/CreateDishCategory';
                        if (!validateType($('input[name="name"]', nRow)[0], 0)) {
                            return;
                        }
                    }
                    formData.append("name", $('#name', nRow)[0].value);
                    formData.append("description", $('#description', nRow)[0].value);
                    formData.append("image", $('#image', nRow)[0].value);
                    //formData.append("isactive", $('#create-switch', nRow)[0].checked);

                    var hm = $("body").wHumanMsg();
                    $.ajax({
                        url: url,  //Server script to process data
                        type: 'POST',
                        // Form data
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        async: false,
                        success: function (data) {
                            if (data != "Error") {
                                hm.wHumanMsg('Đã lưu thành công.', { theme: 'green' });
                                // update data table
                                saveRow(oTable, nEditing, data);
                                // add input hidden if just insert a new row
                                if (new_tr) {
                                    $(nEditing).append('<input type="hidden" id="dish_category_id" name="dish_category_id" value="'+  +'" />');
                                }
                                nEditing = null;
                            } else {
                                hm.wHumanMsg('Lưu thất bại. Xin thử lại.', { theme: 'red' });
                            }
                        },
                        error: function (errMsg) {
                            hm.wHumanMsg('Lưu thất bại. Xin thử lại.', { theme: 'red' });
                        }
                    });
                } else {
                    /* No edit in progress - let's start one */
                    editRow(oTable, nRow, false);
                    nEditing = nRow;
                }
            });
        }

    };

}();