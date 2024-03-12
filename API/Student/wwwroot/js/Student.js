$(document).ready(function () {
    loadData();

});

var loadData = function () {
    $.ajax({
        url: "https://localhost:7260/api/StudentAPI/getallstudents",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.firstName + " " + item.lastName + '</td>';
                html += '<td>' + item.standard + '</td>';
                html += '<td>' + item.city + '</td>';
                html += '<td>' + new Date(item.dob).toLocaleDateString('en-GB') + '</td>';
                html += '<td> <img src="./uploads/' + item.profileImage + '" height="50px" width="50px"/></td>';
                html += '<td>';
                html += '<a href="#" class="btn btn-primary btn-xs me-2" onclick="return getbyID(' + item.id + ')">';
                html += ' Edit';
                html += ' </a>';
                html += '<a href="#" class="btn btn-danger btn-xs" onclick="return Delele(' + item.id + ')">';
                html += 'Delete ';
                html += ' </a>';
                html += '</td>';
                html += '</tr>';

            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText);
        }
    });
}

//Add Data Function
function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var stdObj = {
        Id: $('#ID').val() ? parseInt($('#ID').val()) : 0,
        FirstName: $('#firstname').val(),
        LastName: $('#lastname').val(),
        Standard: parseInt($('#std').val()),
        Address: $('#add').val(),
        City: $("#city").val(),
        State: $("#state").val(),
        Zip: parseInt($("#zip").val()),
        Dob: $("#dob").val()
    };
    console.log(stdObj)

    $.ajax({
        url: "https://localhost:7260/api/StudentAPI/createstudent",
        data: JSON.stringify(stdObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //loadData();
            console.log(result)
            if (result.status) {
                reset();
                loadData();
                toastr.success(result.message);
                $('#myModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
            else {
                toastr.error(result.message);
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText);
        }
    });
}

//Valdidation using jquery
function validate() {
    var isValid = true;
    if ($('#firstname').val().trim() == "") {
        $('#firstname').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#firstname').css('border-color', 'lightgrey');
    }

    if ($('#lastname').val().trim() == "") {
        $('#lastname').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#lastname').css('border-color', 'lightgrey');
    }

    if ($('#std').val().trim() == "") {
        $('#std').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#std').css('border-color', 'lightgrey');
    }
    if ($('#add').val().trim() == "") {
        $('#add').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#add').css('border-color', 'lightgrey');
    }

    if ($('#city').val().trim() == "") {
        $('#city').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#city').css('border-color', 'lightgrey');
    }

    if ($('#state').val().trim() == "") {
        $('#state').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#state').css('border-color', 'lightgrey');
    }
    if ($('#zip').val().trim() == "") {
        $('#zip').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#zip').css('border-color', 'lightgrey');
    }
    if ($('#dob').val().trim() == "") {
        $('#dob').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#dob').css('border-color', 'lightgrey');
    }
    return isValid;
}


//Edit Data function
function getbyID(ID) {
    console.log(ID)
    $.ajax({
        url: "https://localhost:7260/api/StudentAPI/getstudent/" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {           
            $('#ID').val(result.id);
            $('#firstname').val(result.firstName);
            $('#lastname').val(result.lastName);
            $('#std').val(result.standard);
            $('#add').val(result.address);
            $("#city").val(result.city);
            $('#state').val(result.state);
            $('#zip').val(result.zip);
            $("#dob").val(formatDate(result.dob));
            $('#myModal').modal('show');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
            console.log(result)
        }
    });
    return false;
}

//Convert date for edit
function formatDate(d) {
    var date = new Date(parseInt(d.toString().replace('/Date(', '')))
    var dd = date.getDate();
    var mm = date.getMonth() + 1;
    var yyyy = date.getFullYear();
    if (dd < 10) { dd = '0' + dd }
    if (mm < 10) { mm = '0' + mm }
    var ddd = yyyy + '-' + mm + '-' + dd
    return ddd
}

//function for deleting employee's record
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "https://localhost:7260/api/StudentAPI/deletestudent/" + ID,
            type: "DELETE",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                console.log(result)
                if (result.status) {
                    loadData();
                    //window.location.reload(true);
                    toastr.success(result.message);
                    //toastr.success("Employee Deleted Successfully...");

                }
                else {
                    toastr.error(result.message);
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function reset() {
    $('#ID').val("");
    $('#firstname').val("");
    $('#lastname').val("");
    $('#std').val("");
    $('#add').val("");
    $("#city").val("");
    $("#state").val("");
    $("#zip").val("");
    $("#dob").val("");   
}