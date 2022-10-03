function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            srcContent = e.target.result;
        }
        reader.readAsDataURL(input.files[0]);


    }
}

$("#fu1").change(function () {

    if (this.files[0].name != "") {
        readURL(this);
        var aa = $(this)[0].files.length + " Dosya Seçildi..";
        $("#bilgi").empty();
        $("#bilgi").append(aa);
    }
});
$(document).ready(function () {

    (function () {
        var bar = $('.progress-bar');
        var percent = $('.progress-bar');
        var status = $('#status');
        $('#loading').hide();

        $('form').ajaxForm({
            beforeSend: function () {

                status.empty();
                var percentValue = '0%';
                bar.width(percentValue);
                percent.html(percentValue);
            },
            uploadProgress: function (event, position, total, percentComplete) {

                var percentValue = percentComplete + '%';
                bar.width(percentValue);
                percent.html(percentValue);
                $('#loading').show();
            },
            success: function (d) {
                var percentValue = '100%';
                bar.width(percentValue);
                percent.html(percentValue);
                $('#fu1').val('');
                $('#loading').hide();

                if (d.isRedirect) {
                    window.location.href = d.redirectUrl;
                }
            }
        });
    })();


});