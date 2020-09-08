//扩展jquery的格式化方法
$.fn.serializeObject = function () {
    var serializeObj = {};
    var array = this.serializeArray();
    // var str = this.serialize();
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
 */
$.fn.req = function (formId, url, callback, type) {
    $(this).on("click", function () {
        var target = $(this);
        var status = target.attr("data-status");
        if (status == 1) return;
        var text = target.val();
        var data = $(formId).serializeObject();
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
