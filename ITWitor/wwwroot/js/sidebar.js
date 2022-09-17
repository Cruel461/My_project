$(document).ready(function () {
  $('.menuTogglerInput').on('change', function () {
    if ($('.menuTogglerInput').prop('checked')) {
      $('.menu-toggler > a > i').removeClass('fa-arrow-right').addClass('fa-arrow-left');
      $('.sidebar').addClass('smallSidebar');
    } else {
      $('.sidebar').removeClass('smallSidebar');
      $('.menu-toggler > a > i').removeClass('fa-arrow-left').addClass('fa-arrow-right');
    }
  });
});