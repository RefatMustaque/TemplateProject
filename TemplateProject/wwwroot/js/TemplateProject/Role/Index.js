$(document).ready(function () {
    $('#roleTable').dataTable({
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
    });


    /*******************************Event Based *********************************/


    //to delete
    $('body').on('click', '.delete', function () {
        var decision = confirm('Are you sure you want to delete this?');

        var clickedElement = $(this);

        var jsonData = {
            roleId: $(this).attr("data-roleId")
        };

        if (decision) {
            $.ajax({
                url: "/Role/RemoveAsync/",
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

    //to claimTable
    $('body').on('click', '.permissionBtn', function () {

        var clickedElement = $(this);

        var jsonData = {
            roleId: $(this).attr("data-roleId")
        };

            $.ajax({
                url: "/Role/ClaimViewAsync/",
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

    //to claimTable
    $('body').on('click', '.ClaimCheckbox', function () {

        var clickedElement = $(this);

        var jsonData = {
            claimValue: $(this).attr("data-claimValue"),

            roleId: $(this).attr("data-roleId")
        };

        $.ajax({
            url: "/Role/TogglePermission/",
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


function OnSuccessRoleCreate(data) {
    if (data.indexOf("field-validation-error") > -1) {
        $("#roleCreateAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Validation Failed!</div>");
        $("#roleCreateAlert").fadeIn('slow').delay(2000).fadeOut('slow');
    } else {
        $("#roleCreateAlert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Created Successfully!</div>");
        $("#roleCreateAlert").fadeIn('slow').delay(2000).fadeOut('slow');

        $.ajax({
            url: "/Role/GetRoleList/",
            type: "get",
            success: function (response) {
                $("#roleTableDiv").html(response);
                $("#tableAlert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Table Reloaded Successfully!</div>");
                $("#tableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                $('#roleTable').dataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
                });
            },
            error: function (response) {
                $("#tableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed! </strong> Couldn't fetch the data. Something went wrong!</div>");
                $("#tableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });


        //emptyCreateFormFieldOfRegister();
    }
}

function emptyCreateFormFieldOfRegister() {
    $("#Role").val("");
    $("#Email").val("");
    $("#Password").val("");
    $("#ConfirmPassword").val("");
}