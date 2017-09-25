var frontCoverSN = "";

/*編輯相簿文件準備好先將封面SN塞入變數*/
$(document).ready(function () {
    if ($("#frontCover img")[0] != null)    {
        frontCoverSN = $("#frontCover img")[0].id.split("==")[0];
    }
})

//拖曳相關
function allowDrop(ev) {
    ev.preventDefault();
}
//拖曳相關
function drag(ev) {
    ev.dataTransfer.setData("Text", ev.target.id);
}

//多選功能
function imgClick(ev) {
    var imgDivName = document.getElementById(ev.target.id).className,
        shiftRangeparentID = document.getElementById(ev.target.id).parentNode.id;
        shiftRange = $("#" + shiftRangeparentID + " .createAlbumDiv").length,
        isShiftSelect = false,
        initialClassName = "col-md-3 text-center createAlbumDiv",        
        selected = document.getElementById(ev.target.id),
        allSelected = document.getElementsByClassName("col-md-3 text-center");

    //單選功能
    if (imgDivName.indexOf("Active") === -1) {        
        selected.className = initialClassName +" Active";
    }
    else {        
        selected.className = initialClassName;
    }

    //Shift連續選取功能
    if (ev.shiftKey) {
        for (var i = 0; i < shiftRange; i++) {
            if (isShiftSelect === true) {
                if ($("#" + shiftRangeparentID + " .col-md-3.text-center.createAlbumDiv")[i].className.indexOf("Active") != -1) {
                    isShiftSelect = false;
                }
                $("#" + shiftRangeparentID + " .col-md-3.text-center.createAlbumDiv")[i].className = initialClassName +" Active";
                
            }
            else if ($("#" + shiftRangeparentID + " .col-md-3.text-center.createAlbumDiv")[i].className.indexOf("Active") != -1) {
                isShiftSelect = true;

            }
        }
    }
}

//取消圖片新增到相簿
function popDrop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("Text");
    if (document.getElementsByClassName("Active").length !== 0) { 
        multiImg(ev, "NotJoinedPic");
    }
    else{
        if (data) {
            document.getElementById("NotJoinedPic").appendChild(document.getElementById(data));
            checkIsFrontCover(data);//確認取消的圖片不是封面，若是封面就將封面清空
        }
        
    }
}

//將圖片新增到相簿
function addPhoto(ev) {
    var data = ev.dataTransfer.getData("Text");
    ev.preventDefault();
    if (document.getElementsByClassName("Active").length !== 0) {//一次拖拉多檔
        multiImg(ev, "JoinedPic");
    }
    else {
        if (data) {
            document.getElementById("JoinedPic").appendChild(document.getElementById(data));            
        }
    }
}

//拖曳多檔
function multiImg(ev,divID) {
    var Addimg = [];
    for (i = 0; i < document.getElementsByClassName("Active").length; i++) {
        Addimg[i] = document.getElementsByClassName("Active")[i].id;
    }
    for (j = 0; j < Addimg.length; j++) {
        var data = Addimg[j];
        document.getElementById(divID).appendChild(document.getElementById(data));
        document.getElementById(data).className = "col-md-3 text-center createAlbumDiv";
    }
}

//封面用(封面只能單一照片，需要用複製而且是取代的)
function cloneDrop(ev){
    var getID = ev.dataTransfer.getData("Text"),        
        imgSrc = $("#" + getID).css("background-image").split('"')[1];
    ev.preventDefault();
    $("#frontCover").empty(); //清空封面div內的所有元素
    
    $("#frontCover").append("<div id='" + getID + "fronCover' style='background-image:url(" + imgSrc + ");height:80px;'></div>");//複製html到封面的div    
    frontCoverSN = getID;

}

//當圖片移除時，確認圖片封面 多檔不判斷((懶惰
function checkIsFrontCover(data){
    if (frontCoverSN === data){
        $("#frontCover").empty();
        frontCoverSN = "";
    }
}

//取得Querystring  e.g. var s = QueryString("s");
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

//將所有在addArea內的圖片SN放進陣列送到後端
function addimgArray() {
    var imgIDArray = [],
        addContanirID = "JoinedPic";
    if ($("#" + addContanirID).length !== 0){
        for (i = 0; i < $("#" + addContanirID + " div").length; i++) {
            imgIDArray.push($("#" + addContanirID + " div")[i].id);
        }
    }
    return imgIDArray;
}

//存檔
function SaveData(state) {
    var title = $("#txtTitle").val(),
        State = 'Create',
        s = "",
        imgIDArray = []
        IsEnable = $('#chkEnable')[0].checked;

    if (frontCoverSN === "") {
        alert("請選擇封面");
        return false;
    }
    imgIDArray = addimgArray();

    if (state === 1){//state=1為Update
        s = getQueryString("as");
        State = "Update&as=" + s;
    }

    $.ajax({
        type: "post",
        cache: false,
        traditional: true,
        url: "../Tools/albumSystem.ashx?state=" + State ,  //將資料丟到這個頁面
        dataType: "html",
        data: { imgSN: imgIDArray, frontCover: frontCoverSN, Title: title,isEnable:IsEnable },
        success: function (data) {//成功時
            if (state === 0){
                alert("相簿建立成功");
            }
            else if (state === 1){
                alert("相簿更新成功");
            }
            parent.$.fancybox.close();//callback 關閉建立頁面，且讓原頁面重新整理
        },
        error: function () {  //失敗時
            alert("Create Error");
        }
    });
}

//ToolTip系列
function mouseOver(ev) {
    var imgSrc = ev.target.id;
    imgSrc = $("#" + imgSrc).css("background-image").split('"')[1];    
    $("body").append("<div id='tooltip'><img src='" + imgSrc + "'/></div>");
    setToolTipCss(ev);
}
function mouseOut() {
    $("#tooltip").remove();
}
function mouseMove(ev) {
    setToolTipCss(ev);
}
function setToolTipCss(ev) {
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

//全選與取消選取(未實裝)
function selectAll(divObject) {
    var divID = "#" + divObject.id;
    $(divID).find("li").each(function(){
        this.className = "Active";
    })
}
function selectCancel(divObject) {
    var divID = "#" + divObject.id;
    $(divID).find("li").each(function () {
        this.className = "col-md-3 text-center";
    })
}