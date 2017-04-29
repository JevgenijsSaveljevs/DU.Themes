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

        if (error.ValidationErrors) {
            $.each(error.ValidationErrors, function (idx, valErr) {
                toastr.warning(valErr.ErrorMessage);
            })
        }

        if (error.Message) {
            toastr.error(error.Message);
        }

    } catch (err) {
        toastr.error(param2);
    }
    console.log(xhr, param1, param2, param3);
}

function dataTablesDefaults() {

}

function getStatusBadge(text, id) {
    var labelClass = 'label-info';
    switch (id) {
        case 1: labelClass = 'label-info'; break;
        case 6: labelClass = 'label-warning'; break;
        case 10: labelClass = 'label-success'; break;
        case 16: labelClass = 'label-danger'; break;
        default: labelClass = 'label-info'; break;
    }

    var htmlTemplate = '<span class="label {{css}}" style="font-size: small">{{text}}</span>';
    var template = Handlebars.compile(htmlTemplate);

    return template({ css: labelClass, text: text });
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

function updateTextAreaHeight(textAreaElement) {
    textAreaElement.style.height = textAreaElement.scrollHeight + 'px';
}

function updateAllTextAreasHeights() {
    var textAreas = document.getElementsByTagName("textarea");
    for (var i = 0; i < textAreas.length; i++) {
        updateTextAreaHeight(textAreas[i]);
        handleTextAreaKeyPress(textAreas[i]);
    }
}

function handleTextAreaKeyPress(textAreaElement) {
    textAreaElement.onkeypress = function () {
        updateTextAreaHeight(textAreaElement);
    }
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return null;
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}