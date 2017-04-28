function select2OptionsFactory(url, settings) {

    if (!url) {
        throw new Error("url should be provided");
    }

    settings = settings || {};

    var fctry = this;
    this.disabled = settings.disabled;
    this.placeholder = settings.placeholder || this.disabled ? "" : "Ievadiet tekstu";
    this.defaultPageSize = settings.pageSize || 15;
    this.defaultProcessResultsCallback = settings.processResults || function (data, params) {
        var txts = data.items.map(function (input) {
            return {
                text: input.Name,
                id: input.Id,
                entity: input
            };
        });
        params.page = params.page || 1;

        return {
            results: txts,
            pagination: {
                more: (params.page * this.pageSize) < data.total_count
            }
        };
    };

    this.defaultAjaxDataCallback = function (params) {

        return {
            q: params.term || "",
            page: params.page || 1
        };
    };

    createSelect2Options = function () {
        return {
            language:"lv",
            theme: "bootstrap",
            placeholder: fctry.placeholder,
            maximumSelectionLength: 1,
            allowClear: true,
            disabled: fctry.disabled,
            ajax: {
                url: url,
                dataType: 'json',
                delay: 250,
                data: fctry.defaultAjaxDataCallback,
                processResults: fctry.defaultProcessResultsCallback,
                cache: false
            },
            //minimumInputLength: 3,
        }
    }

    return {
        create: createSelect2Options
    }
}