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

let infiniteScrollDefault = () => {
    
    let pageName = location.pathname.split('/')[2],
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
let getPageCount = function () {
    let lastPage = $(".pagination li a").length - 2;
    for (i = 1; i <= lastPage; i++) {
        nextPenSlugs.push($(".pagination li a")[i].innerHTML);        
    }
}

//get Path to Infinte Scroll
var getPenPath = function() {
    let slug = nextPenSlugs[this.loadCount],
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
let getQueryString =  (name) => {
    let hostUrl = window.location.search.substring(1),
        aQueryString = hostUrl.split("&");
    for (i = 0; i < aQueryString.length; i++) {
        var queryString = aQueryString[i].split("=");
        if (queryString[0] === name) {
            return queryString[1];
        }
    }
    return "";
}

//use System Rio_Pic,Rio_Doc
var selectDiv = (ev, docInitialClass, docInitialjQueryClass) => {
    var Div = document.getElementById(ev.target.id).className,
        selected = document.getElementById(ev.target.id),
        isShiftSelect = false,
        shiftRangeparentID = document.getElementById(ev.target.id).id,
        shiftRange = $(docInitialjQueryClass).length;

    if (Div.indexOf("Active") === -1) {
        selected.className = docInitialClass + " Active";
        var createIcon = document.createElement("i");
        createIcon.setAttribute("class", "fa fa-check-square-o");
        createIcon.setAttribute("style", "font-size:48px;");
        document.getElementById(ev.target.id).appendChild(createIcon);        
    }
    else {
        selected.className = docInitialClass;
        $("#" + ev.target.id).children('.fa').remove();
    }

    if (ev.shiftKey) {
        ev.preventDefault();
        console.log(docInitialjQueryClass);
        for (var i = 0; i < shiftRange; i++) {
            if (isShiftSelect === true) {
                if ($(docInitialjQueryClass)[i].className.indexOf("Active") != -1) {
                    isShiftSelect = false;                    
                }
                console.log($(docInitialjQueryClass)[i]);
                $(docInitialjQueryClass)[i].className = docInitialClass + " Active";                
                if ($(docInitialjQueryClass)[i].id != ev.target.id) {
                    var createIcon = document.createElement("i");
                    createIcon.setAttribute("class", "fa fa-check-square-o");
                    createIcon.setAttribute("style", "font-size:48px;");
                    $("#" + $(docInitialjQueryClass)[i].id).append(createIcon);                    
                }
            }
            else if ($(docInitialjQueryClass)[i].className.indexOf("Active") != -1) {
                isShiftSelect = true;
            }
        }
    }
}

//use System selectCover
let radioSelectDiv = (ev, docInitialClass) =>{
    let count = document.getElementsByClassName('picEditDiv Active').length;
    if (ev.target.className.indexOf("Active") != -1) {
        ev.target.className = docInitialClass;
        $('#' + ev.target.id).children().remove();
    }
    else {
        for (i = 0; i < count; i++) {            
            document.getElementsByClassName(docInitialClass + ' Active')[i].className = 'picEditDiv';            
        }
        $('.' + docInitialClass).children().remove();
        ev.target.className += " Active";
        var createIcon = document.createElement("i");
        createIcon.setAttribute("class", "fa fa-check-square-o");
        createIcon.setAttribute("style", "font-size:48px;background-color:white;");
        document.getElementById(ev.target.id).appendChild(createIcon);
    }
}

//use System Rio_Pic,Rio_Doc
//選擇物件
let getSelectFile = function () {
    let selectCount = $(".Active").length,
        SNArray = [];
    for (var i = 0; i < selectCount; i++) {
        SNArray.push($(".Active")[i].id);
    }
    return SNArray;//呼叫刪除用function
}

//use System Rio_Pic,Rio_Doc
//多檔刪除 
let deleteFile = function (type) {

    if (confirm('確認是否刪除檔案?')){
        let SNArray = 0;
        SNArray = getSelectFile();

        if (SNArray == 0){
            alert('請選擇檔案');
            return;
        }

        $.ajax({
            type: 'post',
            cache: false,
            traditional: true,
            url: '../' + type + '/deleteFile',
            dataType: 'html',
            data: { SN: SNArray },
            success: function (data) { //成功時
                alert('檔案刪除成功');
                location.reload();
            },
            error: () =>{  //失敗時
                alert("檔案刪除失敗");
            }
        });
    }
}

//use System Rio_Pic,Rio_Doc
//多檔壓縮
let zipFile = (type) =>{

    let SNArray = getSelectFile();

    if (SNArray.length == 0) {
        alert("請點選檔案");
        return;
    }

    $.ajax({
        type: 'post',
        cache: false,
        traditional: true,
        url: '../Tools/DownloadZip',
        dataType: 'text',
        data: {type: type ,SN: SNArray },
        success: function (data) { //成功時                                        
            location.href = data;
        },
        error:  () => {  //失敗時
            alert("壓縮失敗");
        }
    });
}

//更新個人照
let updateCover =  () => {

    var SN = document.getElementsByClassName('picEditDiv Active')[0].id    
    var type = getQueryString('t');
    $.ajax({
        type: 'post',
        cache: false,
        traditional: true,
        url: '../Rio_Account/selectCover',
        data: {type: type, SN: SN},
        dataType: 'html',
        success: function (data) { //成功時                                        
            alert("存檔成功")
            parent.$.fancybox.close();//callback 關閉建立頁面，且讓原頁面重新整理
        },
        error: () => {  //失敗時
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
        url: '../' + type + '/fileEnable',
        dataType: 'text',
        data: { SN: SNArray },
        success: function (data) { //成功時
            alert(data);
            location.reload();
        },
        error: function () {  //失敗時
            alert("Change Error");
        }
    });    

}

//頁簽
let bookMark =  (ev,divID) => {

    $('#AccountSetting').removeClass();
    $('#IndexSetting').removeClass();
    $('#' + ev.target.id).addClass('btn btn-info');
    $('#' + ev.target.id).siblings().addClass('btn btn-secondary');
    $('.settingContent').hide();
    $('#' + divID).show();
}

let userTrack = () => {
    let vid = getQueryString('vid');    
    $.ajax({
        type: 'post',
        cache: false,
        traditional: true,
        url: '../Rio_Account/userTrack',
        dataType: 'text',
        data: {id: vid},
        success: function (data) { //成功時                                                    
            if (data == 'true') {
                $('#btnUserTrack').val('UnTrack');
            }
            else {
                $('#btnUserTrack').val('Track');
            }            
        },
        error: function () {  //失敗時
            alert("Operating Error");
        }
    });
}