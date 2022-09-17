Date.prototype.toShortTimeString = function (skipZero = false) {
  let timeString = '';
  if (skipZero) {
    let hours = this.getUTCHours() == 0 ? `` : `${this.getUTCHours()}ч. `;
    let minutes = this.getUTCMinutes() == 0 ? `` : `${this.getUTCMinutes()}м. `;
    let seconds = this.getUTCSeconds() == 0 ? `` : `${this.getUTCSeconds()}с.`;
    timeString = `${hours}${minutes}${seconds}`;
  }
  else {
    let hours = this.getUTCHours().toLocaleString().length == 1 && this.getUTCHours() != 0 ? `0${this.getUTCHours()}:` : this.getUTCHours() == 0 ? "00:" : `${this.getUTCHours()}:`;
    let minutes = this.getUTCMinutes().toLocaleString().length == 1 && this.getUTCMinutes() != 0 ? `0${this.getUTCMinutes()}:` : this.getUTCMinutes() == 0 ? "00:" : `${this.getUTCMinutes()}:`;
    let seconds = this.getUTCSeconds().toLocaleString().length == 1 && this.getUTCSeconds() != 0 ? `0${this.getUTCSeconds()}` : this.getUTCSeconds() == 0 ? "00" : `${this.getUTCSeconds()}`;
    timeString = `${hours}${minutes}${seconds}`;
  }
  return timeString;
}

function capitalizeFirstLetter(string) {
  return string.charAt(0).toUpperCase() + string.slice(1);
}

var loadPanel = null;

document.addEventListener('DOMContentLoaded', function () {
  loadPanel = $('.messages-wrapper').dxLoadPanel({
    shadingColor: 'rgba(0,0,0,0.4)',
    position: { of: 'document' },
    visible: false,
    showIndicator: true,
    showPane: true,
    shading: true,
    hideOnOutsideClick: false,
  }).dxLoadPanel('instance');

  document.onreadystatechange = function () {
    if (document.readyState === "complete") {
      loadPanel.hide();
    }
  }

  window.onbeforeunload = function () {
    loadPanel.show();
  }

});


var MessageType = {
  info: "info",
  warning: "warning",
  error: "error",
  success: "success"
}

function showMessage(text, messageType) {
  DevExpress.ui.notify({
    message: text,
    type: messageType,
    displayTime: 4000,
    width: 'auto',
    animation: {
      show: {
        type: 'fade', duration: 400, from: 0, to: 1,
      },
      hide: { type: 'fade', duration: 40, to: 0 },
    },
    closeOnClick: true,
    position: 'top center',
    direction: 'down-push',
  });
}

Array.prototype.remove = function (elem) {
  let index = this.indexOf(elem);
  this.splice(index, 1);
}

function sendAjaxForm(data, url, callback, busyTrigger) {
  function getProps(obj) {
    let formData = new FormData();
    for (var i in obj) {
      formData.append(i, obj[i]);
    }
    return formData;
  }

  let isAsync = callback ? true : false;
  let method = "GET";

  if (data != null && data != undefined) {
    data = data instanceof HTMLFormElement ? new FormData(data) : data instanceof FormData ? data : getProps(data);
    method = "POST";
  }

  if (busyTrigger)
    setTimeout(() => {
      document.body.classList.add('loading');
    }, 0);

  return $.ajax({
    url: url,
    type: method,
    data: data,
    async: isAsync,
    processData: false,
    contentType: false,
    success: function (response) {
      if (busyTrigger) setTimeout(() => { document.body.classList.remove('loading'); }, 0);
      try {
        let obj = JSON.parse(response);
        //if (obj.Url) location.href = obj.Url;
        if (callback) callback(obj);
        if (obj.Message != null)
          showMessage(obj.Message, obj.success ? MessageType.success : MessageType.warning);
      } catch (exc) { console.log(exc); }
    },
    error: function (response) {
      if (busyTrigger) setTimeout(() => { document.body.classList.remove('loading'); }, 0);
      console.log(response);
    },
    progress: function (e) {
      console.log(e);
    }
  });
}

function formToAjax(form, callback, busyTrigger) {
  if (form instanceof HTMLFormElement) {
    form.addEventListener('submit', function (evt) {
      let defaultPrevented = false;
      for (var i = 0; i < this.length - 1; i++) {
        if (this[i].getAttribute('required')) {
          if (this[i].value == '+7 (___) ___-____' || this[i].value == '') {
            defaultPrevented = true;
            DevExpress.ui.notify({
              message: `Необходимо указать адрес Qiwi-кошелька`,
              height: 60,
              width: 350,
              minWidth: 150,
              type: 'warning',
              displayTime: 2500,
              animation: {
                show: {
                  type: 'fade', duration: 400, from: 0, to: 1,
                },
                hide: { type: 'fade', duration: 40, to: 0 },
              },
            },
              {
                position: 'top bottom',
                direction: 'up-push',
              });
          }
        }
      }
      //defaultPrevented = true;

      evt.preventDefault();
      evt.stopPropagation();
      evt.stopImmediatePropagation();
      if (defaultPrevented) return;
      sendAjaxForm(new FormData(this), this.action, callback, busyTrigger);
    });
  }
}

HTMLElement.prototype.toggleClass = function (value) {
  this.className = this.className.includes(value) ? this.className.replace(value, '').trim() : `${value} ${this.className}`;
}

HTMLElement.prototype.removeClass = function (value) {
  this.classList.remove(value);
}

HTMLElement.prototype.addClass = function (value) {
  this.classList.add(value);
}

function sizeAdaptation(videos) {
  videos.forEach(e => {
    e.height = window.innerHeight;
    $(e).css("max-height", window.innerHeight);
  });
  if ($('#sub_categories > div[role=group] .btn-anim.active').length > 0) {
    $('#sub_categories .sizes').css('height', $('#sub_categories .sizes .sizes-inner').height());
  }
}

$.fn.loader = function () {
  return this.each((index, element) => {
    if ($(element).find('i.fa-spinner').length == 0) {
      this.append('<i class="fas fa-spinner fa-3x" style="position:absolute;text-align: center;top: 20px;width:100%;user-select: none;pointer-events: none;"></i>');
    }
  });
};

function scrollHandler(evt) {
  return new Promise((resolve) => {
    if ($(window).scrollTop() > 90) {
      $('.side-toggler').hide();
    }
    else if ($(window).scrollTop() < 90) {
      $('.side-toggler').show();
    }
    if ($(window).scrollTop() > $('.top-bar').height()) {
      $('.top-bar').fadeIn(duration_easing = 50);
      $('.toTop-toggler').addClass('active');
    } else {
      if (url == '/' && detector.isPhoneSized())
        $('.top-bar').fadeOut(duration_easing = 50);
      $('.toTop-toggler').removeClass('active');
    }

    if (detector.isPhoneSized()) {
      if ($(window).scrollTop() > 200) {
        $('#retailcrm-consultant-app').hide();
      }
      else if ($(window).scrollTop() < 200) {
        $('#retailcrm-consultant-app').show();
      }
    }
    resolve();
  });
}