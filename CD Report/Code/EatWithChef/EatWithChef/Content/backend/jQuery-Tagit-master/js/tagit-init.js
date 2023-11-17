//Include in AddNews and UpdateNews

function InitNewsEditor() {
    $("#newsTags").tagit(
        {
            tagSource: function (request, response) {
                $.ajax({
                    type: "GET",
                    url: "/DishManagement/LoadTags",
                    data: { tagName: request.term },
                    dataType: 'json',
                    contentType: "application/json",
                    success: function (tags) {
                        response($.map(tags, function (tag) {
                            return {
                                label: tag.Name,
                                value: tag.Name
                            };
                        }));
                    }
                })
            },
            tagsChanged: function (tag, method) {
                var tags = $("#newsTags").tagit("tags");
                var text = "";
                var id = "";
                if (tags.length > 0) {
                    var i = 0;
                    for (i = 0 ; i < tags.length - 1; i++) {
                        text = text + tags[i].label + ",";
                        id = id + tags[i].value + ",";
                    }
                    text = text + tags[i].label;
                    id = id + tags[i].value;
                }
                $("#taglist").val(text);
                $("#taglistId").val(id);
            },
        });
}


function InitNewsEditorWithPrePopulate(initTags) {
    $("#newsTags_update").tagit(
        {
            tagSource: function (request, response) {
                $.ajax({
                    type: "GET",
                    url: "/DishManagement/LoadTags",
                    data: { tagName: request.term },
                    dataType: 'json',
                    contentType: "application/json",
                    success: function (tags) {
                        response($.map(tags, function (tag) {
                            return {
                                label: tag.Name,
                                value: tag.Name
                            };
                        }));
                    }
                })
            },
            tagsChanged: function (tag, method) {
                var tags = $("#newsTags_update").tagit("tags");
                var text = "";
                var id = "";
                if (tags.length > 0) {
                    var i = 0;
                    for (i = 0 ; i < tags.length - 1; i++) {
                        text = text + tags[i].label + ",";
                        id = id + tags[i].value + ",";
                    }
                    text = text + tags[i].label;
                    id = id + tags[i].value;
                }
                $("#taglist_update").val(text);
                $("#taglistId_update").val(id);
            },
            initialTags: initTags
        });
}