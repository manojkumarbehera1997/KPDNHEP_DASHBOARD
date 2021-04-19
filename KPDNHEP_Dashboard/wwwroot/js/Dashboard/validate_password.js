$(document).ready(function () {
    $('#reset').click(function (e) {
        
        if($('#password').val() != $('#confirmpassword').val()) {
            e.preventDefault();
            $('#error').css("display", "block");
            $('#error').html('<span class="glyphicon glyphicon-info-sign"></span>&nbsp;Password and confirm password do not match');
        }

        var password_regexp = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{7,}$/;
        if(password_regexp.test($('#password').val())) 
        {
            return true;
        }
        else
        { 
            e.preventDefault();
            $('#error').css("display", "block");
            $('#error').html('<span class="glyphicon glyphicon-info-sign"></span>&nbsp;Passwords must be at least seven characters in length with at least one uppercase letter, one lowercase letter and one number.');
            return false;
        }

    });

    $('#update').click(function (e) {
        
        if($('#newpassword').val() != $('#confirmnewpassword').val()) {
            e.preventDefault();
            $('#error').css("display", "block");
            $('#error').html('<span class="glyphicon glyphicon-info-sign"></span>&nbsp;Password and confirm password do not match');
        }

        var password_regexp = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{7,}$/;
        if(password_regexp.test($('#newpassword').val())) 
        {
            return true;
        }
        else
        { 
            e.preventDefault();
            $('#error').css("display", "block");
            $('#error').html('<span class="glyphicon glyphicon-info-sign"></span>&nbsp;Passwords must be at least seven characters in length with at least one uppercase letter, one lowercase letter and one number.');
            return false;
        }

    });

});
