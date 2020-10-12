async function requestData(postData, postUrl) {
    const result = await $.ajax({
        url: postUrl,
        type: 'POST',
        data: postData
    }); 
    return result;
}

function request(postUrl, postData) {
    return $.post(postUrl, postData,
        function (res, status) {
            return res;
        }
    )
}