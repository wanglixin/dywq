//扩展jquery的格式化方法
$.fn.serializeObject = function () {
    var serializeObj = {};
    var array = this.serializeArray();
    console.log(array);
    $(array).each(function () {
        if (serializeObj[this.name]) {
            if ($.isArray(serializeObj[this.name])) {
                serializeObj[this.name].push(this.value);
            } else {
                serializeObj[this.name] = [serializeObj[this.name], this.value];
            }
        } else {
            serializeObj[this.name] = this.value;
        }
    });
    return serializeObj;
};

/**
 * 默认POST
 * @param {any} element
 * @param {any} formId
 * @param {any} url
 * @param {any} callback
 * @param {any} type
 * @param {any} dataHandler 数据处理
 */
$.fn.req = function (formId, url, callback, type, dataHandler) {
    $(this).on("click", function () {
        var target = $(this);
        var status = target.attr("data-status");
        if (status == 1) return;
        var text = target.val();
        var data = $(formId).serializeObject();
        if (dataHandler) {
            data = dataHandler(data);
        }
        $.ajax({
            url: url,
            type: type ? type : 'POST',
            dataType: "json",
            data: JSON.stringify(data),
            headers: { 'Content-Type': 'application/json' },
            beforeSend: function () {
                target.attr("data-status", 1).val("请求中...");
            },
            success: function (r) {
                console.log(r);
                callback(r);
            },
            complete: function (e) {
                target.removeAttr("data-status").val(text);
            }
        })
    });



}


$.fn.uploadImg = function (callback) {
    $(this).on("click", function () {

        var target = $(this);
        var status = target.attr("data-status");
        if (status == 1) return;
        var text = target.val();

        var file = $($(this).attr("data-file"))[0].files[0];
        if (!file) {
            alert("请选择文件上传");
            return;
        };
        console.log(file);
        var formData = new FormData();
        formData.append("file", file);
        formData.append("test", "123456");
        console.log(formData.getAll("file"));
        $.ajax({
            url: '/api/baseapi/UploadImg',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                target.attr("data-status", 1).val("上传中...");
            },
            success: function (r) {
                target.removeAttr("data-status");
                console.log(r);
                if (r.code == 0) {
                    target.val("上传成功");
                } else {
                    target.val("上传失败");
                }
                callback ? callback(r, target) : "";
            },
            complete: function (e) {
                //target.removeAttr("data-status").val(text);
            }
        })

    });
}

var reqPost = function (url, data, callback) {
    $.ajax({
        url: url,
        type: 'POST',
        dataType: "json",
        data: JSON.stringify(data),
        headers: { 'Content-Type': 'application/json' },
        beforeSend: function () {
        },
        success: function (r) {
            callback(r);
        },
        complete: function (e) {
        }
    })
}

var formatDate = function (date) {
    date = date.replace(/T/, " ");
    console.log(date);
    date = new Date(Date.parse(date.replace(/-/g, "/"))); //转换成Data();
    console.log(date);
    var y = date.getFullYear();
    console.log(y);
    var m = date.getMonth() + 1;
    m = m < 10 ? '0' + m : m;
    var d = date.getDate();
    d = d < 10 ? ('0' + d) : d;
    return y + '-' + m + '-' + d;
}

var formatDate2 = function (date) {
    var index = date.indexOf('T');
    return date.substr(0,index);
}



/**
 * 返回上一页并刷新
 * */
var backAndRefresh = function () {
    if (document.referrer == null) {
        history.back();
    } else {
        window.location.replace(document.referrer);
    }

}



var editorInit = function (tinyID) {
    tinymce.init({
        selector: '#' + tinyID,
        language: 'zh_CN',
        plugins: 'print preview searchreplace autolink directionality visualblocks visualchars fullscreen image link media template code codesample table charmap hr pagebreak nonbreaking anchor insertdatetime advlist lists wordcount imagetools textpattern help emoticons autosave bdmap indent2em autoresize formatpainter axupimgs',
        toolbar: 'code undo redo restoredraft | cut copy paste pastetext | forecolor backcolor bold italic underline strikethrough link anchor | alignleft aligncenter alignright alignjustify outdent indent | \
        styleselect formatselect fontselect fontsizeselect | bullist numlist | blockquote subscript superscript removeformat | \
        table image media charmap emoticons hr pagebreak insertdatetime print preview | fullscreen | bdmap indent2em lineheight formatpainter axupimgs',
        height: 650, //编辑器高度
        min_height: 400,
        fontsize_formats: '12px 14px 16px 18px 24px 36px 48px 56px 72px',
        font_formats: '微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Arial Black=arial black,avant garde;Book Antiqua=book antiqua,palatino;',
        importcss_append: true,
        //自定义文件选择器的回调内容
        file_picker_callback: function (callback, value, meta) {
            console.log(value, meta);
            //文件分类
            var filetype = '.pdf, .txt, .zip, .rar, .7z, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .mp3, .mp4';
            //后端接收上传文件的地址
            var upurl = '/api/baseapi/EditorUpload';
            //为不同插件指定文件类型及后端地址
            switch (meta.filetype) {
                case 'image':
                    filetype = '.jpg, .jpeg, .png, .gif';
                    break;
                case 'media':
                    filetype = '.mp3, .mp4';
                    break;
                case 'file':
                default:
            }
            filetype = meta.filetype;
            //模拟出一个input用于添加本地文件
            var input = document.createElement('input');
            input.setAttribute('type', 'file');
            input.setAttribute('accept', filetype);
            input.click();
            input.onchange = function () {
                var file = this.files[0];

                var xhr, formData;
                console.log(file.name);
                xhr = new XMLHttpRequest();
                xhr.withCredentials = false;
                xhr.open('POST', upurl);
                xhr.onload = function () {

                    if (xhr.status != 200) {
                        failure('HTTP Error: ' + xhr.status);
                        return;
                    }
                    var res = JSON.parse(xhr.responseText);
                    if (res.errno != 0) {
                        alert(res.msg);
                        return;
                    }
                    if (meta.filetype == 'file') {
                        callback(res.data, { text: file.name });
                    }
                    // Provide image and alt text for the image dialog
                    if (meta.filetype == 'image') {
                        callback(res.data, { alt: file.name });
                    }
                    // Provide alternative source and posted for the media dialog
                    if (meta.filetype == 'media') {
                        callback(res.data, { poster: file.name });
                    }
                    //callback(res.data);
                };
                formData = new FormData();
                formData.append('file', file, file.name);
                xhr.send(formData);

            };

        },
        autosave_ask_before_unload: false,
    });
}