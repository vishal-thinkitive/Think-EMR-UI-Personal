
//$(document).ready(function () {
//    // Handle tab clicks
//    $('#myTabs a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
//        var tabId = $(e.target).attr('href');
//        loadTabView(tabId);
//    });

//    // Load the initial view for the active tab
//    loadTabView('permissions');
//});

$(document).ready(function () {
    $("#addNewRole").click(function () {
        $('#RoleModal').modal('show');
    })

    function populateSecondDropdown(selectedValue) {
    $.ajax({
        url: '/rolePermissions?roleTypeName=' + selectedValue,
        method: 'GET',
        data: { selectedValue: selectedValue },
        contentType: 'application/json',
        datatype: 'json',
        success: function (data) {
            $("#permissions").empty();
            data.forEach(function (item) {
                $('#permissions').append($("<option></option>").val(item).html(item));
            });
        },
        error: function () {
            console.log("Error in the AJAX request");
        }
        });
    }
    $("#SelectedRole").change(function () {
        var selectedValue = $(this).val();

        populateSecondDropdown(selectedValue);
    });

    $(".select-box").click(function () {
        $(".options").slideToggle();
    });
    var selectedOptions;
    $(".options input[type='checkbox']").change(function () {
        selectedOptions = [];
        var selectedPermissions = []; 
        $(".options input[type='checkbox']:checked").each(function () {
            selectedOptions.push($(this).val());
            selectedPermissions.push($(this).parent().text().trim());
        });

        // Update the text of the select box
        if (selectedPermissions.length > 0) {
            $(".select-box").text(selectedPermissions.join(', '));
        } else {
            $(".select-box").text("Select Permissions");
        }
    });

    var selectedRoleValue;
    $("#SelectedRole").change(function () {
        selectedRoleValue = $(this).val();
    });
    $("#addRoleWithPermissions").click(function () {
        var roleName = $("#roleName").val();
        var intArrayJSON = JSON.stringify(selectedOptions);

        


        $.ajax({
            url: '/addNewRole?roleTypeName=' + selectedRoleValue + '&roleName=' + roleName,
            method: 'POST',
            data: {
                selectedRoleValue: selectedRoleValue,
                roleName: roleName
            },
            contentType: 'application/json',
            datatype: 'json',
            success: function (response) {
                console.log(response);
                $.ajax({
                    url: '/addRoleWithPermission?roleName=' + roleName + '&permissionIds=' + intArrayJSON,
                    method: 'POST',
                    data: {
                        roleName: roleName,
                        selectedOptions: selectedOptions
                    },
                    contentType: 'application/json',
                    datatype: 'json',
                    success: function (response) {
                        console.log(response);
                    },
                    error: function () {
                        console.log("Error in the AJAX request");
                    }
                });
            },
            error: function () {
                console.log("Error in the AJAX request");
            }
        });

        

    });
    $("#closeButton").click(function () {

        $("#SelectedRole").val('');
        $("#roleName").val('');

        $("#options input[type='checkbox']").prop('checked', false);
        $(".options").hide();
        $(".select-box").empty();
    });
});
