var frontCoverSN = "";

/*編輯相簿文件準備好先將封面SN塞入變數*/
$(document).ready(function () {
    if ($("#frontCover img")[0] != null)    {
        frontCoverSN = $("#frontCover img")[0].id.split("==")[0];
    }
})

//拖曳相關
let allowDrop = (ev) => {
    ev.preventDefault();
}
//拖曳相關
let drag =  (ev) => {
    ev.dataTransfer.setData("Text", ev.target.id);
}

//將圖片新增到相簿
let dragPhoto = (sourceID, ev) => {
    let data = ev.dataTransfer.getData("Text"),
        count = 0;

    if (sourceID != ev.target.id) {
        count = $("#JoinedPic div").length + 1;        
        $("#joinPicCount").text(count);
        ev.preventDefault();
        if (document.getElementsByClassName("Active").length !== 0) {//一次拖拉多檔
            multiImg(ev, ev.target.id);
        }
        else {
            if (data) {
                document.getElementById(ev.target.id).appendChild(document.getElementById(data));                
                if (ev.target.id != "NotJoinedPic" && ev.target.id != "JoinedPic") {
                    $('#' + event.target.id).before(document.getElementById(data));
                }
            }
        }
    }
}

//多選功能
let imgClick = function (ev) {
    let imgDivName = document.getElementById(ev.target.id).className,
        shiftRangeparentID = document.getElementById(ev.target.id).parentNode.id;
        shiftRange = $("#" + shiftRangeparentID + " .createAlbumDiv").length,
        isShiftSelect = false,
        initialClassName = "col-md-4 createAlbumDiv",
        selected = document.getElementById(ev.target.id),
        allSelected = document.getElementsByClassName("col-md-3 text-center");
    //單選功能
    if (imgDivName.indexOf("Active") === -1) {
        selected.className = initialClassName + " Active";
        
        var createIcon = document.createElement("i");
        createIcon.setAttribute("class", "fa fa-check-square-o");
        createIcon.setAttribute("style", "font-size:28px;");
        document.getElementById(ev.target.id).appendChild(createIcon);           
    }
    else {
        selected.className = initialClassName;
        $("#" + ev.target.id).children().remove();
    }
    
    //Shift連續選取功能
    if ($('.Active').length > 1) {
        if (ev.shiftKey) {            
            for (let i = 0; i < shiftRange; i++) {
                if (isShiftSelect === true) {
                    if ($(".col-md-4.createAlbumDiv")[i].className.indexOf("Active") != -1) {
                        isShiftSelect = false;
                    }
                    $(".col-md-4.createAlbumDiv")[i].className = initialClassName + " Active";
                    if ($(".col-md-4.createAlbumDiv")[i].id != ev.target.id) {
                        let createIcon = document.createElement("i");
                        createIcon.setAttribute("class", "fa fa-check-square-o");
                        createIcon.setAttribute("style", "font-size:28px;");
                        $("#" + $(".col-md-4.createAlbumDiv")[i].id).append(createIcon);
                    }

                }
                else if ($(".col-md-4.createAlbumDiv")[i].className.indexOf("Active") != -1) {
                    isShiftSelect = true;

                }
            }
        }
    }
}

//拖曳多檔
let multiImg = (ev, divID) => {
    let Addimg = [];

    for (i = 0; i < document.getElementsByClassName("Active").length; i++) {
        Addimg[i] = document.getElementsByClassName("Active")[i].id;
    }
    for (j = 0; j < Addimg.length; j++) {
        let data = Addimg[j];
        document.getElementById(divID).appendChild(document.getElementById(data));
        if (ev.target.id != "NotJoinedPic" && ev.target.id != "JoinedPic") {
            $('#' + event.target.id).before(document.getElementById(data));
        }
        document.getElementById(data).className = "col-md-3 createAlbumDiv";
        $('#' + data).children().remove();
    }
}

//封面用(封面只能單一照片，需要用複製而且是取代的)
let cloneDrop =  (ev) => {
    let getID = ev.dataTransfer.getData("Text"),        
        imgSrc = $("#" + getID).css("background-image").split('"')[1];
    ev.preventDefault();
    $("#frontCover").empty(); //清空封面div內的所有元素
    
    $("#frontCover").append("<img id='" + getID + "==fronCover' src='" + imgSrc + "'>");//複製html到封面的div    
    frontCoverSN = getID;

}

//取得Querystring  e.g. var s = QueryString("s");
let getQueryString = (name) => {  
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

//將所有在addArea內的圖片SN放進陣列送到後端
let addimgArray =  () => {
    let imgIDArray = [],
        addContanirID = "JoinedPic";
    if ($("#" + addContanirID).length !== 0){
        for (i = 0; i < $("#" + addContanirID + " div").length; i++) {
            imgIDArray.push($("#" + addContanirID + " div")[i].id);
        }
    }
    return imgIDArray;
}

//存檔
let SaveData = (state) => {
    let title = $("#txtTitle").val(),
        State = 'Create',
        SN = "",
        imgIDArray = []
        IsEnable = $('#chkEnable')[0].checked;


    if ($("#txtTitle").val() === "") {
        alert("相簿名稱不得空白")
        return false;
    }

    if (state == 'UpdateAlbum'){
        SN = getQueryString("aSN");        
    }

    imgIDArray = addimgArray();

    if (frontCoverSN === "") {
        alert("請選擇封面");
        return false;
    }

    if (imgIDArray.indexOf(frontCoverSN) == -1) {        
        $("#frontCover").empty();
        frontCoverSN = "";
        alert("封面不得選擇相簿內以外的圖片")
        return false;
    }
    

    $.ajax({
        type: "post",
        cache: false,
        traditional: true,        
        url: "../Rio_Album/" + state,
        data: { imgSN: imgIDArray, frontCover: frontCoverSN, Title: title, isEnable: IsEnable, AlbumSN: SN},
        success: function (data) {//成功時
            if (state === 0){
                alert(data);
            }
            else if (state === 1){
                alert(data);
            }
            parent.$.fancybox.close();//callback 關閉建立頁面，且讓原頁面重新整理
        },
        error: function () {  //失敗時
            alert("Error");
        }
    });
}

//ToolTip系列
let mouseOver = (ev) => {
    var imgSrc = ev.target.id;
    imgSrc = $("#" + imgSrc).css("background-image").split('"')[1];    
    $("body").append("<div id='tooltip'><img src='" + imgSrc + "'/></div>");
    setToolTipCss(ev);
}
let mouseOut =  () => {
    $("#tooltip").remove();
}
let mouseMove = (ev) => {
    setToolTipCss(ev);
}
let setToolTipCss = (ev) => {
    var x = 20,
        y = -100;
    if (ev.target.parentElement.id == "JoinedPic") {
        x = -350;
    }

    $("#tooltip").css({
        left: ev.pageX + x + "px",
        top: ev.pageY + y + "px",
        position: "absolute"
    }).fadeIn("slow", "swing");
}
