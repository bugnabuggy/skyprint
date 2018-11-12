export function parsingUrl(url) {
  return url.split('&')[1].split('=')[1];
}