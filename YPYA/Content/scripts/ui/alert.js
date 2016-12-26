/**
 * Sweetalerts demo page
 */
(function($) {
  'use strict';

  $('.demo1').on('click', function() {
    swal('Here\'s a message!');
  });

  $('.demo2').on('click', function() {
    swal({
      title: 'Auto close alert!',
      text: 'I will close in 2 seconds.',
      timer: 2000
    });
  });

  $('.demo3').on('click', function() {
    swal('Here\'s a message!', 'It\'s pretty, isn\'t it?');
  });

  $('.demo4').on('click', function() {
    swal('Good job!', 'You clicked the button!', 'success');
  });

  $('.demo5').on('click', function () {
      var secili = $(this);
      var projeId = $(this).attr("projeId");
    swal({
      title: 'Emin misin?',
      text: 'Sil tu\u015funa basarsanız projenizi sonsuza dek kaybedeceksiniz!',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#DD6B55',
      confirmButtonText: 'Evet Sil!',
      cancelButtonText: 'İptal',
      closeOnConfirm: false,
      html: true
    }, function () {
        $.post("../Home/ProjeSil", { projeId: projeId }, function () {
            secili.parent().parent().remove();
            swal({
                title: 'Silindi!',
                text: 'Projeniz ba\u015farıyla silindi!',
                type: 'success',
                confirmButtonText: 'Tamam'
            });
        });
    });
  });

  $('.demo11').on('click', function () {
      var secili = $(this);
      var projeId = $(this).attr("projeId");
      swal({
          title: 'Emin misin?',
          text: 'Reddet tu\u015funa basarsanız teklifi geri çevireceksiniz!',
          type: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#DD6B55',
          confirmButtonText: 'Evet Reddet!',
          cancelButtonText: 'İptal',
          closeOnConfirm: false,
          html: true
      }, function () {
          $.post("../Home/ProjeRed", { projeId: projeId }, function () {
              secili.parent().parent().remove();
              swal({
                  title: 'Reddedildi!',
                  text: 'Proje ba\u015farıyla reddedildi!',
                  type: 'success',
                  confirmButtonText: 'Tamam'
              });
          });
      });
  });

  $('.demo12').on('click', function () {
      swal({
          title: 'Dikkat?',
          text: 'Bu sürecin is takip bilgileri bulunmaktadır',
          type: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#DD6B55',
          confirmButtonText: 'Yeni sürece aktar',
          cancelButtonText: 'Sil',
          closeOnConfirm: false,
          closeOnCancel: false,
          html: true
      }, function (isConfirm) {
          if (isConfirm) {
              VeriKaydet(levelKontrol, baslik, baslangic, bitis, parentSurecId, projeId,2   )
          } else {
              VeriKaydet(levelKontrol, baslik, baslangic, bitis, parentSurecId, projeId,1);
          }
      });
  });

  $('.demo6').on('click', function() {
    swal({
      title: 'Are you sure?',
      text: 'You will not be able to recover this imaginary file!',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#DD6B55',
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, cancel plx!',
      closeOnConfirm: false,
      closeOnCancel: false
    }, function(isConfirm) {
      if (isConfirm) {
        swal('Deleted!', 'Your imaginary file has been deleted!', 'success');
      } else {
        swal('Cancelled', 'Your imaginary file is safe :)', 'error');
      }
    });
  });

  $('.demo7').on('click', function() {
    swal({
      title: 'Sweet!',
      text: 'Here\'s a custom image.',
      imageUrl: 'images/avatar.jpg'
    });
  });

  $('.demo8').on('click', function() {
    swal({
      title: 'HTML <small>Title</small>!',
      text: 'A custom <span style=\"color:#F8BB86\">html<span> message.',
      html: true
    });
  });

  $('.demo9').on('click', function() {
    swal({
      title: 'An input!',
      text: 'Write something interesting:',
      type: 'input',
      showCancelButton: true,
      closeOnConfirm: false,
      animation: 'slide-from-top',
      inputPlaceholder: 'Write something'
    }, function(inputValue) {
      if (inputValue === false) {
        return false;
      }
      if (inputValue === '') {
        swal.showInputError('You need to write something!');
        return false;
      }
      swal('Nice!', 'You wrote: ' + inputValue, 'success');
    });
  });

  $('.demo10').on('click', function() {
    swal({
      title: 'Ajax request example',
      text: 'Submit to run ajax request',
      type: 'info',
      showCancelButton: true,
      closeOnConfirm: false,
      showLoaderOnConfirm: true
    }, function() {
      setTimeout(function() {
        swal('Ajax request finished!');
      }, 2000);
    });
  });

})(jQuery);
