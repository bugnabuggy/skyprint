export function parsingUrl(url) {
  const tempArray = url.split('&');
  let idOrder = '';
  tempArray.forEach( item => {
    if (item.indexOf('zakaz') >= 0)
    {
      idOrder = item.split('=')[1];
    }
  });
  return idOrder;
}

export function getDataFromResponse( response ) {
  if ( response.hasOwnProperty( 'data' ) ) {
    const data = response.data;
    if ( data.hasOwnProperty( 'data' ) ) {
      return data.data;
    } else {
      return data;
    }
  } else {
    return response;
  }
}