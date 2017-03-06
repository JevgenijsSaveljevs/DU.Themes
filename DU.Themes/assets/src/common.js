function findEntity(result) {
    return result.find(function (element) {
        if (element.type) {
            if (element.type === "DATA_LOADED") {
                return true;
            }
        }

        return false;
    });
}

function getData(url, callback) {
    $.ajax({
        url: url,
        method: "GET",
        contentType: "application/json; UTF8",
    }).done(function (data) {
        callback(null, {
            type: "DATA_LOADED",
            data: data
        });
    }).error(function (xhr, param1, param2, param3) {
        toastr.error(param2);
        console.log(xhr, param1, param2, param3);
    }).always(function () {
    });
}

function handleError(xhr, param1, param2, param3) {
    try {
        var error = JSON.parse(xhr.responseText);
        console.log('error is:', error);

        if (error.ValidationErrors) {
            $.each(error.ValidationErrors, function (idx, valErr) {
                toastr.error(valErr.ErrorMessage);
            })
        }

        if (error.Message) {
            toastr.error(error.Message);
        }

        if (error.GeneralError) {
            toastr.error(error.GeneralError);
        }

    } catch (err) {
        toastr.error(param2);
    }
    console.log(xhr, param1, param2, param3);
}

function dataTablesDefaults() {

}


dataTablesDefaults.prototype.language = {
    "processing": "Uzgaidiet...",
    "search": "Meklēt:",
    "lengthMenu": "Rādīt _MENU_ ierakstus",
    "info": "Parādīti _START_. līdz _END_. no _TOTAL_ ierakstiem",
    "infoEmpty": "Nav ierakstu",
    "infoFiltered": "(atlasīts no pavisam _MAX_ ierakstiem)",
    "infoPostFix": "",
    "loadingRecords": "Iekraušanas ieraksti ...",
    "zeroRecords": "Nav atrasti vaicājumam atbilstoši ieraksti",
    "emptyTable:": "Tabula nav datu",
    "paginate": {
        "first": "Pirmā",
        "previous": "Iepriekšējā",
        "next": "Nākošā",
        "last": "Pēdējā"
    },
    "aria": {
        "sortAscending": ": aktivizēt kolonnu, lai kārtotu augošā",
        "sortDescending": ": aktivizēt kolonnu, lai kārtotu dilstošā"
    }
};

var dt = new dataTablesDefaults();

function navigationHelper() {

    var redirect = function (url) {
        window.location.href = url;
    }

    return {
        redirect: redirect
    }
}

var navigation = new navigationHelper();