async function requestData(postData, postUrl) {
    const result = await $.ajax({
        url: postUrl,
        type: 'POST',
        data: postData
    }); 
    return result;
}

