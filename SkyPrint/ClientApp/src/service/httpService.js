import axios from 'axios';

export function httpGet(url) {
  return axios.get(url);
}

export function httpPost(url, data) {
  return axios.post(url, data);
}