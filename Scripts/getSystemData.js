//import picture from './pictureContact';

$(document).ready(function () {
    var systemName = window.location.pathname.split('/')[1];
    var Obj = getJSON(systemName);

    console.log(Obj.obj);

    ReactDOM.render(
        <PictureView data={Obj} />,
        document.getElementById('sysDiv')
    );    
});

var getJSON = function (systemName) {
    let rawData;
    $.ajax({
        type: 'GET',
        url: '../' + systemName + '/getViewJson',
        async: false,
        contentType: false,
        processData: false,
        dataType: 'json',
        success: function (data) {//成功時
            rawData = data;
        }
    });

    return rawData;
};


class PictureView extends React.Component {
    render() {
        var picNode = this.props.data.map(function (node, i) {
            return (
                <div class="col-md-3" key={i}>
                    <div id="node.SN" class="picDivView" style="background-image:url(node.PicPath/Scaling/node.PicName)" onclick="selectDiv(event,'picDivView','.picDivView')">
                        <a class="fancybox" data-fancybox="images" style="display:block;" rel="ligthbox" href="node.PicPath/node.PicName" title="node.PicName"></a>
                    </div>
                </div>
            )
        })
        return (
            <div>
                {picNode}
            </div>
        )
    }
}
