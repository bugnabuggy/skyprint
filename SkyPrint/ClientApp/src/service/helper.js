export function parsingUrl(url) {
  return url.split('&')[1].split('=')[1];
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