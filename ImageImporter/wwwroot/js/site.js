// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

$(function () {
    AfterLoad($(this));
});

function AfterLoad(loadedObj) {

    //This is used to dynamically load content on the pages. 
    //See /ticket/details for a few examples.
    loadedObj.find("*.GetPartial").each(function (index, value) {
        var obj = $(this);
        var uri = obj.attr('data-uri');
        var id = obj.attr('data-id');

        $.ajax({
            type: "GET",
            url: uri,
            contentType: "application/json; charset=utf-8",
            data: { "Id": id },
            datatype: "json",
            success: function (data) {
                obj.html(data);
                AfterLoad(obj);
            },
            error: function (response) {
                obj.html("Error code: " + response.status);
            }
        });
    });


    loadedObj.find("*.GetPartialOnClick").each(function (index, value) {
        var obj = $(this);
        var contents = obj.find(".Contents");


        obj.find("*.LoadAction").each(function (index, value) {
            var loadAction = $(this);
            loadAction.click(function (e) {
                var uri = loadAction.attr('data-uri');
                var id = loadAction.attr('data-id');
                e.preventDefault();
                LoadModal(uri, id);
            });
        });

        function LoadModal(uri, id) {
            $.ajax({
                type: "GET",
                url: uri,
                contentType: "application/json; charset=utf-8",
                data: { "Id": id },
                datatype: "json",
                success: function (data) {
                    contents.html(data);
                    AfterLoad(contents);
                },
                error: function (response) {
                    contents.html("Error code: " + response.status);
                }
            });
        }
    });



    //Loads a partial to the modal and shows the modal.
    //See /person/index for an example
    loadedObj.find("*.LoadModal").each(function (index, value) {
        var obj = $(this);
        var uri = obj.attr('data-uri');
        var id = obj.attr('data-id');
        var showAction = obj.find(".ShowAction");
        var showActionDbl = obj.find(".ShowActionDbl");
        var modal = $(".GeneralModal");
        var modalContent = modal.find('.modal-content');
        
        showAction.click(function (e) {
            e.preventDefault();
            LoadModal();
        });
        showActionDbl.dblclick(function (e) {
            e.preventDefault();
            LoadModal();
        });

        function LoadModal() {
            $.ajax({
                type: "GET",
                url: uri,
                contentType: "application/json; charset=utf-8",
                data: { "Id": id },
                datatype: "json",
                success: function (data) {
                    modalContent.html(data);
                    modal.modal('show');
                    AfterLoad(modalContent);
                },
                error: function (response) {
                    modalContent.html("Error code: " + response.status);
                    modal.modal('show');
                    AfterLoad(modalContent);
                }
            });
        }
    });

    //This makes an input field editable, allowes one to edit and safe without reloading the page.
    //Examples are in /shared/_EditableTextArea
    loadedObj.find("*.EditableField").each(function (index, value) {
        var obj = $(this);
        var id = obj.attr('data-id');
        var uri = obj.attr('data-uri');
        var editValue = obj.find(".EditValue");
        var viewValue = obj.find(".ViewValue");
        var editAction = obj.find(".EditAction");
        var editActionDbl = obj.find(".EditActionDbl");
        var cancelAction = obj.find(".CancelAction");
        var saveAction = obj.find(".SaveAction");
        var editField = obj.find(".EditField");
        var viewField = obj.find(".ViewField");

        editActionDbl.dblclick(function (e) {
            editField.show();
            viewField.hide();
            e.preventDefault();
        });

        editAction.click(function (e) {
            editField.show();
            viewField.hide();
            e.preventDefault();
        });

        cancelAction.click(function (e) {
            viewField.show();
            editField.hide();
            e.preventDefault();
        });

        saveAction.click(function (e) {
            var data = {};
            data[`ParentId`] = id;
            data[`Text`] = editValue.val();
            e.preventDefault();
            $.ajax({
                url: uri,
                data: JSON.stringify(data),
                type: "POST",
                dataType: 'html',
                contentType: "application/json",
                success: function (rxdata) {
                    viewValue.html(rxdata);
                    viewField.show();
                    editField.hide();
                    AfterLoad(obj);
                },
                error: function (response) {
                    alert("Error code: " + response.status);
                }
            });
        });
    });


    loadedObj.find("*.PostValue").each(function (index, value) {
        var obj = $(this);
        var id = obj.attr('data-id');
        var uri = obj.attr('data-uri');
        var reload = obj.attr('reload');
        var valueObj = obj.find(".Value");
        var postAction = obj.find(".PostAction");
        var postActionChange = obj.find(".PostActionChange");
        var postActionDbl = obj.find(".PostActionDbl");

        postActionDbl.dblclick(function (e) {
            e.preventDefault();
            Post();
        });

        postAction.click(function (e) {
            e.preventDefault();
            Post();
        });

        postActionChange.change(function (e) {
            e.preventDefault();
            Post();
        });

        function Post() {
            var data = {};
            data[`ParentId`] = id;
            data[`Text`] = valueObj.val();
            $.ajax({
                url: uri,
                data: JSON.stringify(data),
                type: "POST",
                dataType: 'html',
                contentType: "application/json",
                success: function (rxdata) {
                    if (reload) {
                        location.reload();
                    }
                },
                error: function (response) {
                    alert("Error code: " + response.status);
                }
            });
        }
    });

    //https://stackoverflow.com/questions/45007712/bootstrap-4-dropdown-with-search
    loadedObj.find("*.SearchDropdown").each(function (index, value) {
        var obj = $(this);
        var id = obj.attr('data-id');
        var uri = obj.attr('data-uri');
        var selectedField = obj.find(".SelectedField");
        var searchField = obj.find(".SearchField");
        var searchValue = obj.find(".SearchValue");
        var emptyMsg = obj.find(".Empty");
        var errorMsg = obj.find(".Error");
        var menuItems = obj.find(".MenuItems");
        emptyMsg.show();
        errorMsg.hide();

        //Enter search mode
        selectedField.click(function (e) {
            selectedField.hide();
            searchField.show();
            Search("");
        });

        //Search value entered
        searchValue.on('input propertychange', () => {
            var value = searchValue.val();
            if (value) {
                Search(value);
            }
            else {
                emptyMsg.show();
                menuItems.empty();
            }
        });

        //Start searching
        function Search(value) {
            $.ajax({
                type: "GET",
                url: uri,
                contentType: "application/json; charset=utf-8",
                data: { "id": id, "search": value },
                datatype: "json",
                success: function (data) {
                    HandleSearchResults(data);
                },
                error: function (response) {
                    errorMsg.html("Error code: " + response.status);
                    errorMsg.show();
                    emptyMsg.hide();
                }
            });
        }

        //Search results returned from server
        function HandleSearchResults(data) {
            errorMsg.hide();
            emptyMsg.show();
            menuItems.empty();
            $.each(data, function (key, item) {
                if (item.postUri) {
                    menuItems.append('<a class="dropdown-item" href="#" data-id="' + item.itemId + '" data-value="' + item.itemValue + '" data-uri="' + item.postUri + '" data-pid="' + item.parentId + '">' + item.itemValue + '</a>');
                } else {
                    menuItems.append('<a class="dropdown-item" href="#" data-id="' + item.itemId + '" data-value="' + item.itemValue + '">' + item.itemValue + '</a>');
                }
                emptyMsg.hide();
            });
        }

        //Search result selected
        menuItems.on('click', '.dropdown-item', (e) => {
            e.preventDefault();
            var iobj = $(event.target);
            var selectedId = iobj.attr('data-id');
            var selectedValue = iobj.attr('data-value');
            var postUri = iobj.attr('data-uri');
            var parentId = iobj.attr('data-pid');

            if (postUri) {
                PostSelection(selectedId, postUri, parentId);
            } else {
                //In case we don't want to send the selected item right away, but wait for a submit button oid.
                selectedField.html(selectedValue);
                selectedField.attr("data-id", selectedId);
                //Exit searchmode
                selectedField.show();
                searchField.hide();
            }
        });

        //Post the selected item
        function PostSelection(selectedId, postUri, parentId) {
            var data = {};
            data[`ParentId`] = parentId;
            data[`itemId`] = selectedId;
            $.ajax({
                url: postUri,
                data: JSON.stringify(data),
                type: "POST",
                dataType: 'html',
                contentType: "application/json",
                success: function (rxdata) {
                    location.reload();
                },
                error: function (response) {
                    alert("Error code: " + response.status);
                }
            });
        }
    });


    //https://stackoverflow.com/questions/45007712/bootstrap-4-dropdown-with-search
    loadedObj.find("*.SearchDropdownEvents").each(function (index, value) {
        var obj = $(this);
        var uri = obj.attr('data-uri');
        var searchObj = obj.find(".SearchValue");
        var resultsObj = obj.find(".SearchResults");
        var emptyMsg = obj.find(".Empty");
        var errorMsg = obj.find(".Error");

        emptyMsg.show();
        errorMsg.hide();

        //Search value entered
        searchObj.on('input propertychange', () => {
            var value = searchObj.val();
            if (value) {
                Search(value);
            }
            else {
                emptyMsg.show();
                resultsObj.empty();
            }
        });

        //Start searching
        function Search(value) {
            $.ajax({
                type: "GET",
                url: uri,
                contentType: "application/json; charset=utf-8",
                data: { "search": value },
                datatype: "json",
                success: function (data) {
                    HandleSearchResults(data);
                },
                error: function (response) {
                    errorMsg.html("Error code: " + response.status);
                    errorMsg.show();
                    emptyMsg.hide();
                }
            });
        }

        //Search results returned from server
        function HandleSearchResults(data) {
            errorMsg.hide();
            emptyMsg.show();
            resultsObj.empty();
            $.each(data, function (key, item) {
                resultsObj.append('<a class="dropdown-item" href="#" data-id="' + item.itemId + '" data-value="' + item.itemValue + '">' + item.itemValue + '</a>');
                emptyMsg.hide();
            });
        }

        //Search result selected
        resultsObj.on('click', '.dropdown-item', (e) => {
            e.preventDefault();
            var iobj = $(event.target);
            searchObj.val(iobj.attr('data-value'));
            obj.attr('data-selected', iobj.attr('data-id'));
        });
    });

    loadedObj.find("*.EventLoader").each(function (index, value) {
        var obj = $(this);
        var selector = obj.find(".EventSelector");
        var details = obj.find(".EventDetails");
        var id = obj.find("#LifeEventId").val();

        $("#submit").click(function (e) {
            e.preventDefault();
            //console.log($('form'));
            var form = $(this).parents('form:first')[0];
            //console.log(form);
            //var form = $('form')[0];
            var formData = new FormData(form);

            $(".Roles").find("*.SomeRole").each(function (index, value) {
                var role = $(value);
                var roleSelect = role.find(".RoleTypes");
                var e = roleSelect.find("option:selected");
                var personId = role.find(".SearchDropdownEvents").attr("data-selected");
                formData.append("Roles[" + index + "].RoleType", e.val());
                formData.append("Roles[" + index + "].Person.PersonId", personId);

            });

            $.ajax({
                method: 'post',
                url: "/Events/Edit",
                data: formData,
                processData: false,
                contentType: false,
                success: function () {
                    location.reload();
                },
                error: function (response) {
                    alert("Error code: " + response.status);
                }
            });
        });

        selector.on('change', function () {
            var selectedValue = this.value;
            LoadDetails(selectedValue);
            UpdateRoles();
        });


        LoadDetails(selector.val());
        UpdateRoles();

        $(".AddRole").click(function () {
            var newRole = $(`
                <div class="SomeRole input-group mb-3">
                    <div class="SearchDropdownEvents" data-uri="/Events/SearchPersons" data-selected="0">
                    <input class="dropdown-toggle form-control SearchValue" type="text" data-toggle="dropdown" placeholder="Search" />
                        <div class="dropdown-menu">
                            <div class="SearchResults"></div>
                            <div class="Empty dropdown-header">No items found</div>
                            <div class="Error dropdown-header">An error occured</div>
                        </div>
                    </div>
                    <select class="custom-select input-group-append RoleTypes">
                    </select>
                    <div class="input-group-append">
                        <button class="btn btn-outline-secondary RemoveRole" type="button">Remove</button>
                    </div>
                </div>
            `);
            AfterLoad($(".Roles").append(newRole));
            UpdateRoles();
        });

        function UpdateRoles() {
            LoadRoles(selector.val(), function (data) {
                $(".Roles").find("*.SomeRole").each(function (index, value) {
                    var role = $(value);
                    var roleSelect = role.find(".RoleTypes");
                    var beforeVal = roleSelect.val();
                    roleSelect.empty();
                    $(data).each(function (key, value) {
                        var option = $('<option></option>').attr("value", value["item2"]).text(value["item1"]);
                        if (beforeVal == value["item1"] || beforeVal == value["item2"])
                            option = $('<option selected></option>').attr("value", value["item2"]).text(value["item1"]);
                        roleSelect.append(option);
                    });

                    role.find(".RemoveRole").click(function () {
                        role.remove();
                    });
                });
            });
        }


        function LoadDetails(type) {
            $.ajax({
                type: "GET",
                url: "/Events/GetDetailsPartial",
                contentType: "application/json; charset=utf-8",
                data: { "Id": id, "Type": type },
                datatype: "json",
                success: function (data) {
                    details.html(data);
                    AfterLoad(details);
                },
                error: function (response) {
                    details.html("Error code: " + response.status);
                }
            });
        }

        function LoadRoles(type, callback) {
            $.ajax({
                type: "GET",
                url: "/Events/GetRoleTypes",
                contentType: "application/json; charset=utf-8",
                data: { "Id": id, "Type": type },
                datatype: "json",
                success: callback,
                error: function (response) {
                    alert("Error code: " + response.status);
                }
            });
        }
    });








}







