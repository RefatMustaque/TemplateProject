$(document).ready(function () {
    $('#userTable').dataTable({
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
    });

    $('#Role').select2();


    /*******************************Event Based *********************************/

    //dropdown ajax for mp activity grid info
    $('body').on('change', '#Role', function (e) {

        var jsonData = {
            roleName: $('#Role').val()
        };

        $.ajax({
            url: "/Account/GetUserListByRole/",
            type: "get",
            data: jsonData,
            success: function (response) {
                $("#userTableDiv").html(response);
                $('#userTable').dataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
                });
                $('#Role').select2();
            },
            error: function (response) {
                $("#userTableDiv").html(response);
            }
        });
        e.stopImmediatePropagation();
    });

    //to delete
    $('body').on('click', '.delete', function () {
        var decision = confirm('Are you sure you want to delete this?');

        var clickedElement = $(this);

        var jsonData = {
            userId: $(this).attr("data-userId")
        };

        if (decision) {
            $.ajax({
                url: "/Account/Remove/",
                type: "get",
                data: jsonData,
                success: function (response) {
                    if (response === true) {
                        clickedElement.closest("tr").remove();
                        $("#tableAlert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Deleted Successfully!</div>");
                        $("#tableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                    }
                },
                error: function (response) {
                    $("#tableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Couldn't delete. Something went wrong!</div>");
                    $("#tableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                }
            });
        }
    });


    //to remove From Role
    $('body').on('click', '.removeFromRole', function () {
        var decision = confirm('Are you sure you want to remove user from this role?');

        var clickedElement = $(this);

        var jsonData = {
            userId: $(this).attr("data-userId"),
            roleId: $(this).attr("data-roleId")
        };

        if (decision) {
            $.ajax({
                url: "/Account/RemoveFromRole/",
                type: "get",
                data: jsonData,
                success: function (response) {
                    if (response === true) {
                        clickedElement.closest("tr").remove();
                        $("#userRoleTableAlert")
                            .html(
                                "<div class='alert alert-success' style='color:black'><strong> Success!</strong> Removed from role successfully!</div>");
                        $("#userRoleTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                    } else {
                        $("#userRoleTableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Couldn't remove from role. Something went wrong!</div>");
                        $("#userRoleTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                    }
                },
                error: function (response) {
                    $("#userRoleTableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Couldn't remove from role. Something went wrong!</div>");
                    $("#userRoleTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                }
            });
        }
    });

    //For Setting Permissions
    $('body').on('click', '.permissionBtn', function () {

        var clickedElement = $(this);

        var jsonData = {
            userId: $(this).attr("data-userId")
        };

        $.ajax({
            url: "/Account/ClaimViewAsync/",
            type: "get",
            data: jsonData,
            success: function (response) {
                $("#claimViewDiv").html(response);

                $('#claimTable').dataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
                });
            },
            error: function (response) {
                $("#tableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Couldn't fetch the claim. Something went wrong!</div>");
                $("#tableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
    });

    //For Adding Permission to user
    $('body').on('click', '.permissionBtn', function () {

        var clickedElement = $(this);

        var jsonData = {
            userId: $(this).attr("data-userId")
        };

        $.ajax({
            url: "/Account/ClaimViewAsync/",
            type: "get",
            data: jsonData,
            success: function (response) {
                $("#claimViewDiv").html(response);

                $('#claimTable').dataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
                });
            },
            error: function (response) {
                $("#tableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Couldn't fetch the claim. Something went wrong!</div>");
                $("#tableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
    });

    //For Adding Role to user
    $('body').on('click', '.addToRoleBtn', function () {

        var clickedElement = $(this);

        var jsonData = {
            userId: $(this).attr("data-userId")
        };

        $.ajax({
            url: "/Account/UserRole/",
            type: "get",
            data: jsonData,
            success: function (response) {
                $("#addRoleViewDiv").html(response);

                $('#userRoleTable').dataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
                });
            },
            error: function (response) {
                $("#userRoleCreateAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Something went wrong!</div>");
                $("#userRoleCreateAlert").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
    });


    //to claimTable
    $('body').on('click', '.ClaimCheckbox', function () {

        var clickedElement = $(this);

        var jsonData = {
            claimValue: $(this).attr("data-claimValue"),

            userId: $(this).attr("data-userId")
        };

        $.ajax({
            url: "/Account/TogglePermission/",
            type: "get",
            data: jsonData,
            success: function (response) {
                if (response === true) {
                    $("#claimTableAlert").html("<div class='alert alert-success' style='color:black'><strong> Success! </strong> Successfully toggled the permission!</div>");
                    $("#claimTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                } else {
                    $("#claimTableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Couldn't toggled the permission. Something went wrong!</div>");
                    $("#claimTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                }

            },
            error: function (response) {
                $("#claimTableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Couldn't toggled the permission. Something went wrong!</div>");
                $("#claimTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
    });




});


function OnSuccessRegister(data) {
    if (data.indexOf("field-validation-error") > -1) {
        $("#registerAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Validation Failed!</div>");
        $("#registerAlert").fadeIn('slow').delay(2000).fadeOut('slow');
    } else {
        $("#registerAlert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Created Successfully!</div>");
        $("#registerAlert").fadeIn('slow').delay(2000).fadeOut('slow');
        $('#userTable').dataTable({
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
        });
        emptyCreateFormFieldOfRegister();
    }
}

function OnSuccessUserRole(data) {
    if (data.indexOf("field-validation-error") > -1) {
        $("#userRoleCreateAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Validation Failed!</div>");
        $("#userRoleCreateAlert").fadeIn('slow').delay(2000).fadeOut('slow');
    } else {
        $("#userRoleCreateAlert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Added Successfully!</div>");
        $("#userRoleCreateAlert").fadeIn('slow').delay(2000).fadeOut('slow');
        $('#userRoleTable').dataTable({
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
        });
        emptyCreateFormFieldOfRegister();
    }
}
