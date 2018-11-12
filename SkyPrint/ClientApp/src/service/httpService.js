import axios from 'axios';

export function httpGet(url) {
  return axios.get(url);
}