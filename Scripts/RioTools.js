var nextPenSlugs = [];
$(document).ready(function () {

    //fancybox
    fancyboxDefault();

    //-------------------------------------//
    // init Infinte Scroll

    infiniteScrollDefault();


})

function fancyboxDefault() {
    $(".various").fancybox({
        maxWidth: 1920,
        maxHeight: 1024,
        fitToView: false,
        width: '85%',
        height: '85%',
        autoSize: false,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none',
        afterClose: function () { window.location.reload(); }
    });
}

function infiniteScrollDefault() {
    
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
    console.log(appendTag);    
}

//get page to Infinte Scroll
function getPageCount() {
    var lastPage = $(".pagination li a").length - 2;
    for (i = 1; i <= lastPage; i++) {
        nextPenSlugs.push($(".pagination li a")[i].innerHTML);        
    }
}

//get Path to Infinte Scroll
function getPenPath() {
    var slug = nextPenSlugs[this.loadCount],
        m = 'V',
        pageName = location.pathname.split('/')[2],
        path = '',
        SN = '0';
    if (slug) {
        if (pageName == 'RioPicView' || pageName == 'RioDocView'){
            m = getQueryString("m");
            path = pageName + '?m=' + m + '&page=' + slug;
        }
        else if (pageName == 'RioAlbumView') {  
            path = pageName + '?page=' + slug;
        }
        else if (pageName == 'RioAlbumContent') {
            SN = getQueryString("SN");
            path = pageName + '?SN=' + SN + '&page=' + slug;
        }
        return path;
    }
}

//get Querystring  e.g. var s = QueryString("s");
function getQueryString(name) {
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
function selectDiv(ev, docInitialClass, docInitialjQueryClass) {
    var docDiv = document.getElementById(ev.target.id).className,
        selected = document.getElementById(ev.target.id),
        isShiftSelect = false,
        shiftRangeparentID = document.getElementById(ev.target.id).id,
        shiftRange = $(docInitialjQueryClass).length;

    if (docDiv.indexOf("Active") === -1) {
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

//use System Rio_Pic,Rio_Doc
function getSelectFile() {
    var selectCount = $(".Active").length;
    var SNArray = [];
    for (var i = 0; i < selectCount; i++) {
        SNArray.push($(".Active")[i].id);
    }
    return SNArray;//呼叫刪除用function
}

//use System Rio_Pic,Rio_Doc
function deleteFile(type) {

    var SNArray = getSelectFile();

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

//use System Rio_Pic,Rio_Doc
function zipFile(type) {

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
