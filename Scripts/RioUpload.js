function Allowdrag(ev) { //檔案拖曳結束
    event.preventDefault();
}
function Drop(ev) { //檔案拖曳中
    event.preventDefault();
    var files = ev.dataTransfer.files;//取得檔案
    FileType = document.getElementById('hidUploadType').value,//取得可上傳附檔名
        FileTypeArray = FileType.replace(/\*./g, "").split(';'),//將可上傳附檔名切割成陣列來判斷
        ErrorFileList = "",//錯誤訊息
        successCount = 0,//檔案上傳成功的數量
        fileArryLength = 0;


    for (i = 0; i < files.length; i++) { //依據檔案數量一個一個往Server上丟
        fileArryLength = files[i].name.split('.').length;//有檔名內有多個. 所以要知道該檔名的附檔名的話這邊是用split切割取最後一個為附檔名

        if ($.inArray(files[i].name.split('.')[fileArryLength - 1].toLowerCase(), FileTypeArray) !== -1) { //判斷檔案類型是否為可上傳類型
            SaveFiles(files[i].name, files[i]);//呼叫存檔涵式
            successCount += 1;
        }
        else {
            ErrorFileList += files[i].name + "\n";
        }
    }

    if (ErrorFileList !== "") {//錯誤訊息
        alert(ErrorFileList + '上傳失敗' + '\n' + '上傳檔案的類型必須是' + FileType);
    }
    else {
        alert('上傳結束，共上傳成功' + successCount + '個檔案');
    }    
}
function SaveFiles(fileName, files) //存檔用
{
    var fd = new FormData(),
        t = getQueryString("t");
    fd.append(fileName, files);

    $.ajax({
        type: 'POST',
        url: 'fmUpload.ashx?t=' + t,  //將資料丟到這個頁面
        async:false,
        contentType: false,
        processData: false,
        data: fd,
        success: function (data) {         //成功時
            var fr = new FileReader();
            if (t === "img") {
                fr.onload = createImgTag;
            }
            else if (t === "Doc") {
                fr.onload = createDocTag(t, fileName);
            }
            fr.readAsDataURL(files);
        },
        error: function () {  //失敗時
            alert("error： FileName= " + fileName);
        }
    });
}
function createImgTag(ev) { //顯示圖片縮圖用
    var x = ev.target.result,//將圖片轉成base64
        FileTag = document.createElement('img');//建立img元素
    FileTag.src = x;//指定元素連結
    document.getElementById('UploadArea').appendChild(FileTag);
}

function createDocTag(t, fileName) { //顯示文件icon用
    var FileTag = document.createElement("img"),//建立img元素
        Extansion = fileName.split(".")[fileName.split('.').length - 1],        
        fileNameDiv = document.createElement("div"),
        fileNamep = document.createElement("p"),
        fileNameDivText = document.createTextNode(fileName);

    if (Extansion === "doc" || Extansion === "docx")
        FileTag.src = "../../Content/img/icon/doc.ico";

    if (Extansion === "xls" || Extansion === "xlsx")
        FileTag.src = "../../Content/img/icon/xls.ico";

    if (Extansion === "ppt" || Extansion === "pptx")
        FileTag.src = "../../Content/img/icon/ppt.ico";
       
    fileNameDiv.className = "uploadDocDiv";
    fileNameDiv.appendChild(FileTag);
    fileNamep.appendChild(fileNameDivText);
    fileNameDiv.appendChild(fileNamep);

    document.getElementById("UploadArea").appendChild(fileNameDiv);
}

function getQueryString(name) {  //取得Querystring  e.g. var s = QueryString("s");
    var hostUrl = window.location.search.substring(1);
    var aQueryString = hostUrl.split("&");
    for (var i = 0; i < aQueryString.length; i++) {
        var queryString = aQueryString[i].split("=");
        if (queryString[0] === name) {
            return queryString[1];
        }
    }
    return "";
}