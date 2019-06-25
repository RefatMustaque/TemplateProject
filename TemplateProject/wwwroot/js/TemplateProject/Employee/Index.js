$(document).ready(function() {

    $('#JoinDate').datepicker({
        autoclose:true
    });

    $('#employeeTable').dataTable();

    $("#BranchList").select2();

    /***************************************Event Based ************************************/

    $('body').on('click', '#formClearButton', function (e) {

        $.ajax({
            url: "/Employee/Clear/",
            type: "get",
            success: function (response) {
                $("#employeeFormDiv").html(response);
                $('#JoinDate').datepicker({
                    autoclose: true
                });
            },
            error: function (response) {
                $("#employeeAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't clear the form. Something went wrong!</div>");
                $("#employeeAlert").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
        e.stopImmediatePropagation();
    });

    //get employee filtering Branch
    $('body').on('change', '#BranchList', function (e) {

        var jsonData = {
            BranchId: $('#BranchList').val()
        };

        $.ajax({
            url: "/Employee/EmployeeListGridView/",
            type: "get",
            data: jsonData,
            success: function (response) {
                $("#employeeTableDiv").html(response);
                $('#employeeTable').dataTable();
                $("#BranchList").select2();
            },
            error: function (response) {
                $("#employeeTableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't get the employe of Branch. Something went wrong!</div>");
                $("#employeeTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
        e.stopImmediatePropagation();
    });

    //user acceess
    $("body").on('click', '.userAccess', function () {
        var jsonData = {
            employeeId: $(this).attr("data-employeeId")
        };

        $.ajax({
            url: "/Employee/UserAccess/",
            type: "get",
            data: jsonData,
            success: function (response) {
                $("#employeeUserAccessDiv").html(response);
            },
            error: function (response) {
                $("#employeeUserAccessDiv").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't get the user access info. Something went wrong!</div>");
                $("#employeeUserAccessDiv").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
    });

    //delete employee
    $('body').on('click', '.delete', function (e) {
        var decision = confirm('Are you sure you want to delete this?');
        var clickedElement = $(this);

        var jsonData = {
            employeeId: $(this).attr("data-employeeId")
        };

        if (decision) {
            $.ajax({
                url: "/Employee/DeleteEmployee/",
                type: "get",
                data: jsonData,
                success: function (response) {
                    if (response === true) {
                        clickedElement.closest("tr").remove();
                        $("#employeeTableAlert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Deleted Successfully!</div>");
                        $("#employeeTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                    }
                },
                error: function (response) {
                    $("#employeeTableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't delete the info. Somtheing went wrong!</div>");
                    $("#employeeTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
                }
            });
            e.stopImmediatePropagation();
        }

    });

    //edit employee
    $('body').on('click', '.btnForEdit', function () {
        var clickedElement = $(this);
        var jsonData = {
            employeeId: $(this).attr("data-employeeId")
        };

        $.ajax({
            url: "/Employee/EditView/",
            type: "get",
            data: jsonData,
            success: function (response) {
                $("#employeeEditFormDiv").html(response);
                $('#editFormJoinDate').datepicker({
                    autoclose: true
                });
            },
            error: function (response) {
                $("#employeeEditFormDiv").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't get the info for edit. Something went wrong!</div>");
                $("#employeeEditFormDiv").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
        //e.stopImmediatePropagation();
    });
});

function OnSuccessEmployeeCreate(data) {
    if (data.indexOf("field-validation-error") > -1) {
        $("#employeeAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Validation Failed!</div>");
        $("#employeeAlert").fadeIn('slow').delay(2000).fadeOut('slow');
        $('#JoinDate').datepicker({
            autoclose: true
        });
    } else {
        $("#employeeAlert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Created Successfully!</div>");
        $("#employeeAlert").fadeIn('slow').delay(2000).fadeOut('slow');
        $('#JoinDate').datepicker({
            autoclose: true
        });
        //emptyCreateForm();
    }
}


function OnFailureEmployeeCreate() {
    $("#employeeAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't insert the data. Something went wrong!</div>");
    $("#employeeAlert").fadeIn('slow').delay(2000).fadeOut('slow');
}


function OnSuccessEmployeeEdit(data) {
    if (data.indexOf("field-validation-error") > -1) {
        $("#employeeEditAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Validation Failed!</div>");
        $("#employeeEditAlert").fadeIn('slow').delay(2000).fadeOut('slow');
        
        $('#editFormJoinDate').datepicker({
            autoclose: true
        });
    } else {
        $("#employeeEditAlert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Updated Successfully!</div>");
        $("#employeeEditAlert").fadeIn('slow').delay(2000).fadeOut('slow');
        $('#editFormJoinDate').datepicker({
            autoclose: true
        });
        var jsonData = {
            BranchId: $('#BranchList').val()
        };

        $.ajax({
            url: "/Employee/EmployeeListGridView/",
            type: "get",
            data: jsonData,
            success: function (response) {
                $("#employeeTableDiv").html(response);
                $('#employeeTable').dataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    "columnDefs": [
                        { "width": "15%", "targets": 0 },
                        { "width": "18%", "targets": 1 },
                        { "width": "18%", "targets": 2 },
                        { "width": "17%", "targets": 3 },
                        { "width": "17%", "targets": 4 },
                        { "width": "15%", "targets": 5 }
                    ]
                });
                $("#BranchList").select2();
            },
            error: function (response) {
                $("#employeeTableAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't get the employe of Branch. Something went wrong!</div>");
                $("#employeeTableAlert").fadeIn('slow').delay(2000).fadeOut('slow');
            }
        });
       
        //emptyCreateForm();
    }
}

function OnFailureEmployeeEdit() {
    $("#employeeAlert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't update the data. Something went wrong!</div>");
    $("#employeeAlert").fadeIn('slow').delay(2000).fadeOut('slow');
}





function emptyCreateForm() {
    $("#BranchId").val("");
    $("#Name").val("");
    $("#Gender").val("");
    $("#JoinDate").val("");
}


function OnSuccessUserAccess(data) {
    if (data.indexOf("field-validation-error") > -1) {
        $("#userAccessAlert")
            .html(
                "<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Validation Failed!</div>");
        $("#userAccessAlert").fadeIn('slow').delay(2000).fadeOut('slow');
    } else {
        $("#userAccessAlert")
            .html(
                "<div class='alert alert-success' style='color:black'><strong> Success!</strong> Updated Successfully!</div>");
        $("#userAccessAlert").fadeIn('slow').delay(2000).fadeOut('slow');
    }
}

function OnFailureUserAccess() {
    alert("Failed!");
}