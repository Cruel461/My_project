
$(document).ready(function () {
  document.body.removeClass('loading');

});

$(function () {
  $('.labelholder').labelholder();
  $('.fancybox').fancybox();

  $('*:not(a)[data-toggle="tooltip"]').tooltip();

  $('a[data-toggle="tooltip"]').tooltip({
    animated: "fade",
    html: true,
  });

  $(".dx-textbox").each((index, element) => {
    let $this = $(element);

    if ($this.closest('.html-editor').length == 0) {
      let textBoxOptions = {
        value: $this.attr('value'),
        placeholder: $this.attr('placeholder'),
        name: $this.attr('name'),
      }
      if (element.getAttribute('required')) {
        textBoxOptions.elementAttr = { required: true };
        textBoxOptions.inputAttr = { required: true };
      }
      if ($this.attr('type') == 'tel') {
        textBoxOptions.mask = '+7 (X00) 000-0000';
        textBoxOptions.maskRules = { X: /[02-9]/ };
      }

      $this.dxTextBox(textBoxOptions);
    }
  });

  $('.dx-button').each((index, button) => {
    let $this = $(button);
    let type = $this.attr('type') != undefined ? $this.attr('type') : 'default';
    $this.dxButton({
      text: $this.attr('text'),
      type: type,
      useSubmitBehavior: $this.attr('action') == 'submit' ? true : false,
    });
  });

  $(".dx-checkbox").each((index, element) => {
    let $this = $(element);
    let val = $this.attr('box-value');
    $this.dxCheckBox({
      value: val == 'true' ? true : false,
    });
  });

  $('.itw-selectbox').each(function (index, elem) {
    let $this = $(this);
    if ($this.closest('.html-editor').length == 0) {
      let dataAttr = $this.attr('items');
      let data = dataAttr ? JSON.parse($this.attr('items')) : null;


      console.log(data);

      let propertyAttr = this.getAttribute('property');

      var name;
      if (propertyAttr)
        name = propertyAttr;

      var displayName = this.getAttribute('label') ? this.getAttribute('label') : null;

      var value;
      if (data)
        value = data.find(e => e.id == $this[0].getAttribute('value'));

      if (value)
        valueId = value.id;
      if (data) {
        $this.dxSelectBox({
          dataSource: data,
          valueExpr: 'id',
          displayExpr: 'OriginalName',
          inputAttr: {
            name: name
          },
          value: value ? value.id : null,
          placeholder: value ? value.OriginalName : null,
          label: displayName
        });
      }
    }
  })



  $('.messages-wrapper .alert').each((i, e) => {
    let $msg = $(e);
    $msg.find('button').on('click', () => {
      $msg.fadeOut(350, () => $msg.remove());
    });
    setTimeout(() => { $msg.fadeOut(350, () => $msg.remove()); }, 4000);
  });

  $('#reset_password').on('click', function (evt) {
    evt.stopImmediatePropagation();
    evt.stopPropagation();
    evt.preventDefault();
    sendAjaxForm(null, '/ResetPassword', (response) => {
      if (response.Html)
        $('.form-wrapper').html(response.Html);
      if (!response.Success) {
        showMessage(response.Message, 'red');
      }
      else if (response.Message) {
        showMessage(response.Message, 'yellow');
      }

    }, false);
  });
});
