window.fbAsyncInit = function () {
    FB.init({
        appId: '179417776247588',
        //appId: '688929598124633',
        xfbml: true,
        version: 'v3.0'
    });
    FB.AppEvents.logPageView();
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}

function statusChangeCallback(response) {
    if (response.status === 'connected') {  //登陸狀態已連接    
        getUserInfo(response.authResponse.accessToken);
    }
    else {
        console.log('登入失敗');
    }

}
//獲取用户信息
function getUserInfo(fbToken) {
    FB.api('/me', 'get', {
        access_token: fbToken,
        fields: 'id, name, email'
    }, function (response) {
        //response.id / response.name / response.email
        //把用户token信息交給後台
        //self.location = 'fbLogin?id=' + response.id + '&name=' + response.name + '&email=' + response.email;        
        $.ajax({
            type: 'POST',
            url: '../Tools/fbLogin.ashx?id=' + response.id + '&name=' + response.name + '&email=' + response.email,
            async: false,
            contentType: false,
            processData: false,
            success: function (data) {         //成功時
                history.go(0);
            },
            error: function () {  //失敗時
                alert("error： FB登入失敗");
            }
        });
    });
}

function fb_login() {
    // FB 第三方登入，要求公開資料與email
    FB.login(function (response) {
        statusChangeCallback(response);
    }, { scope: 'public_profile,email' });
}