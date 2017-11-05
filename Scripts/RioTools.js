var nextPenSlugs = [];
$(document).ready(function () {

    $("[data-fancybox]").fancybox({        
        afterClose: function () {
            history.go(0);
        },
        iframe: {            
            css: {
                width: '1600px',
                height: '800px'
            }
        }

    });
    // init Infinte Scroll
    infiniteScrollDefault();
})

var infiniteScrollDefault = function () {
    
    var pageName = location.pathname.split('/')[2],
        appendTag = '.col-md-3';
        getPageCount();    

    if (pageName == 'RioPicView' || pageName == 'RioAlbumView' || pageName == 'RioAlbumContent') {
        appendTag = '.col-md-3';
    }
    else if (pageName == 'RioDocView'){
        appendTag = '.col-md-2';
    }    
    $('.form-group').infiniteScroll({
        path: getPenPath,
        append: appendTag,
        status: '.pagination-container',
    });    
}

//get page to Infinte Scroll
var getPageCount = function () {
    var lastPage = $(".pagination li a").length - 2;
    for (i = 1; i <= lastPage; i++) {
        nextPenSlugs.push($(".pagination li a")[i].innerHTML);        
    }
}

//get Path to Infinte Scroll
var getPenPath = function () {
    var slug = nextPenSlugs[this.loadCount],
        pageName = location.pathname.split('/')[2],
        vid = '',
        m = 'm',
        path = '',
        SN = '0';
    if (slug) {
        if (pageName == 'RioPicView' || pageName == 'RioDocView'){
            vid = getQueryString('vid');
            m = getQueryString("m");            
            path = pageName + '?vid=' + vid + '&m=' + m + '&page=' + slug;
        }
        else if (pageName == 'RioAlbumView') {  
            vid = getQueryString('vid');
            path = pageName + '?vid=' + vid + '&page=' + slug;
        }
        else if (pageName == 'RioAlbumContent') {
            SN = getQueryString("SN");
            vid = getQueryString('vid');
            path = pageName + '?vid=' + vid + '&SN=' + SN + '&page=' + slug;
        }
        return path;
    }
    return '';

}

//get Querystring  e.g. var s = QueryString("s");
var getQueryString = function (name) {
    var hostUrl = window.location.search.substring(1);
    var aQueryString = hostUrl.split("&");
    for (i = 0; i < aQueryString.length; i++) {
        var queryString = aQueryString[i].split("=");
        if (queryString[0] === name) {
            return queryString[1];
        }
    }
    return "";
}

//use System Rio_Pic,Rio_Doc
var selectDiv = function (ev, docInitialClass, docInitialjQueryClass) {
    var Div = document.getElementById(ev.target.id).className,
        selected = document.getElementById(ev.target.id),
        isShiftSelect = false,
        shiftRangeparentID = document.getElementById(ev.target.id).id,
        shiftRange = $(docInitialjQueryClass).length;

    if (Div.indexOf("Active") === -1) {
        selected.className = docInitialClass + " Active";
    }
    else {
        selected.className = docInitialClass;
    }

    if (ev.shiftKey) {
        ev.preventDefault();
        for (var i = 0; i < shiftRange; i++) {
            if (isShiftSelect === true) {
                if ($(docInitialjQueryClass)[i].className.indexOf("Active") != -1) {
                    isShiftSelect = false;
                }
                $(docInitialjQueryClass)[i].className = docInitialClass + " Active";

            }
            else if ($(docInitialjQueryClass)[i].className.indexOf("Active") != -1) {
                isShiftSelect = true;
            }
        }
    }

}

//use System selectCover
var radioSelectDiv = function (ev, docInitialClass) {
    var count = document.getElementsByClassName('col-md-3  thumbnail picEditDiv Active').length;
    if (ev.target.className.indexOf("Active") != -1) {
        ev.target.className = docInitialClass;
    }
    else {
        for (i = 0; i < count; i++) {            
            document.getElementsByClassName(docInitialClass + ' Active')[i].className = 'col-md-3 thumbnail picEditDiv';
        }
        
        ev.target.className += " Active";
    }
}

//use System Rio_Pic,Rio_Doc
//選擇物件
var getSelectFile = function () {
    var selectCount = $(".Active").length;
    var SNArray = [];
    for (var i = 0; i < selectCount; i++) {
        SNArray.push($(".Active")[i].id);
    }
    return SNArray;//呼叫刪除用function
}

//use System Rio_Pic,Rio_Doc
//多檔刪除 
var deleteFile = function (type) {

    if (confirm('確認是否刪除檔案?')){
        var SNArray = 0;
        SNArray = getSelectFile();

        if (SNArray == 0){
            alert('請選擇檔案');
            return;
        }

        $.ajax({
            type: 'post',
            cache: false,
            traditional: true,
            url: '../Tools/deleteFile.ashx?t=' + type,
            dataType: 'html',
            data: { SN: SNArray },
            success: function (data) { //成功時
                alert('檔案刪除成功');
                location.reload();
            },
            error: function () {  //失敗時
                alert("檔案刪除失敗");
            }
        });
    }
}

//use System Rio_Pic,Rio_Doc
//多檔壓縮
var zipFile = function (type) {

    var SNArray = getSelectFile();

    if (SNArray.length == 0) {
        alert("請點選檔案");
        return;
    }

    $.ajax({
        type: 'post',
        cache: false,
        traditional: true,
        url: '../Tools/Addzip.ashx?t=' + type,
        dataType: 'html',
        data: { SN: SNArray },
        success: function (data) { //成功時                                        
            location.href = data;
        },
        error: function () {  //失敗時
            alert("壓縮失敗");
        }
    });
}

//更新個人照
var updateCover = function () {

    var SN = document.getElementsByClassName('col-md-3 thumbnail picEditDiv Active')[0].id    
    var type = getQueryString('t');
    $.ajax({
        type: 'post',
        cache: false,
        traditional: true,
        url: '../Tools/selectCover.ashx?t=' + type + '&s=' + SN,
        dataType: 'html',
        success: function (data) { //成功時                                        
            alert("存檔成功")
            parent.$.fancybox.close();//callback 關閉建立頁面，且讓原頁面重新整理
        },
        error: function () {  //失敗時
            alert("存檔失敗");
        }
    });
}

//改變啟用狀態
var fileEnable = function (type) {
    
    var SNArray = 0;
    SNArray = getSelectFile();

    if (SNArray == 0) {
        alert('請選擇檔案');
        return;
    }

    $.ajax({
        type: 'post',
        cache: false,
        traditional: true,
        url: '../Tools/fileEnable.ashx?t=' + type,
        dataType: 'html',
        data: { SN: SNArray },
        success: function (data) { //成功時
            alert('改變啟用狀態成功');
            location.reload();
        },
        error: function () {  //失敗時
            alert("改變啟用狀態成功");
        }
    });    

}

//頁簽
var bookMark = function (ev,divID) {

    $('.settingBookMark').removeClass('bookMarkSelect');
    $('#' + ev.target.id).addClass('bookMarkSelect');
    $('.settingContent').hide();
    $('#' + divID).show();
}