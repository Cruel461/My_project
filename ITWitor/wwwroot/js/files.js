(function ($) {
  var idArray = [];

  function loadedHandler() {
    $('#files tbody .fas.fa-trash').on('click', function () {
      let fileId = this.parentElement.parentElement.getAttribute("data-id");
      var formData = new FormData();
      formData.append('id', fileId);
      sendAjaxForm(formData, "/Admin/RemoveFile", (response) => { console.log(response); document.querySelector(`tr[data-id='${fileId}']`).remove() }, true);
    });

    $('#file_upload_form .fa.fa-plus').on('click', function () {
      let filePicker = this.parentElement.querySelector('input[type=file]');
      filePicker.addEventListener('change', () => {

        setTimeout(() => {
          document.body.classList.add('loading');
        }, 1);

        var formData = new FormData(document.querySelector('#file_upload_form'));

        sendAjaxForm(formData, "/Admin/UploadFile", (response) => {

          location.reload();

        }, true);
      });
      filePicker.click();
    });




    //$("#files thead .tablesorter-header-inner input").on('click', function () {
    //    let triggers = $(this).closest('table').find('tbody input[type=checkbox]');
    //    let mainCheckBox = this;
    //    triggers.each(function () {
    //        this.checked = mainCheckBox.checked;
    //        let rowId = $(this).closest("tr").attr('data-id');
    //        let temp = idArray.find(e => { e == rowId });
    //        if (this.checked) idArray.push(rowId);
    //    });
    //});


    $('#files thead input[type=checkbox]').on('change', function () {
      document.querySelectorAll('#files tbody input[type=checkbox]').forEach(e => {
        e.checked = this.checked;
        let rowId = $(e).closest("tr").attr('data-id');
        if (e.checked) {
          idArray.push(rowId)
        }
        else {
          idArray.remove(rowId);
        }
      });
    });

    $('#files tbody input[type=checkbox]').on('change', function () {
      let rowId = $(this).closest("tr").attr('data-id');
      let temp = idArray.find(e => { e == rowId });
      if (this.checked && idArray) idArray.push(rowId);
      else idArray.remove(rowId);
    });

    $('#files thead .fas.fa-trash').on('click', function () {
      if (!idArray || idArray.length == 0) return;

      let data = { "idArray": idArray };

      setTimeout(() => {
        document.body.classList.add('loading');
      }, 0);

      $.ajax({
        type: 'POST',
        data: data,
        url: '/Admin/RemoveFile',
        dataType: "json",
        async: true,
        success: function () {
          document.location.reload();
        },
        xhr: function () {
          var xhr = $.ajaxSettings.xhr();
          xhr.addEventListener('progress', function (evt) {
            if (evt.lengthComputable) {
              var percentComplete = Math.ceil(evt.loaded / evt.total * 100);
              console.log(percentComplete);
              //progressBar.val(percentComplete);
            }
          }, false);

          return xhr;

        }
      });
    });
    $('table .fa.fa-copy').on('click', function () {
      let input = this.parentElement.querySelector('input');;
      console.log(input);
      input.select();
      document.execCommand('copy');
      showMessage("Ссылка скопирована в буфер", "green");
    });
  }

  return loadedHandler();
})(jQuery);