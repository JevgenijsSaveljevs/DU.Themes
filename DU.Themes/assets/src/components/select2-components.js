Vue.component('select2', {
    props: ['url', 'settings', 'value', 'displayProp'],
    template: '#select2-template',
    mounted: function () {
        var factory = new select2OptionsFactory(this.url, this.settings),
            options = factory.create(),
            vm = this,
            element = $(this.$el).val(this.value);

        var propName = this.displayProp || "Id";

        if (this.value && this.value[propName]) {
            var data = [{ id: this.value.Id, text: this.value[propName], entity: this.value }];
            options.data = data;
        }

        var select = element.select2(options);

        //console.log('select2 : :select', this.value);

        select.on('select2:select', function (customEvent) {
            vm.$emit('input', customEvent.params.data.entity);
        });

        select.on('select2:unselect', function (customEvent) {
            vm.$emit('input', null);
        });
    },
    watch: {
        value: function (value) {
            $(this.$el).val(value)
        }
    },
});
