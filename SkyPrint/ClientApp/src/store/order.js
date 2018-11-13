import { httpGet, httpPost } from '../service/httpService';
import { orderDataUrl } from '../service/api';
import { getDataFromResponse } from '../service/helper';

const LOAD_ORDER_DATA = 'LOAD_ORDER_DATA';
const LOADING_ORDER_DATA = 'LOADING_ORDER_DATA';
const SEND_AMENDMENTS = 'SEND_AMENDMENTS';
const SHOW_AMENDMENTS_MODAL = 'SHOW_AMENDMENTS_MODAL';
const ORDER_NOT_FOUND = 'ORDER_NOT_FOUND';
export const initialStateOrder = {
  name: '',
  picture: '',
  info: '',
  address: '',
  hasClientAnswer: false,
  status: '',
  isLoading: false,
  isShowAmendmentsModal: false,
  isOrderNotFound: false,
};

export const actionCreators = {
  loadDataAction: (idOrder) => (dispatch) => {
    dispatch({ type: LOADING_ORDER_DATA, isLoading: true });
    httpGet(orderDataUrl.replace('%id%', idOrder))
      .then((response) => {
        dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
        dispatch({ type: LOAD_ORDER_DATA, data: getDataFromResponse(response) });
      })
      .catch((error) => {
        if (error.response.status === 404) {
          dispatch({ type: ORDER_NOT_FOUND, isNotFound: true });
        }
        dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
      });
  },
  sendAmendmentsAction: (idOrder, message, file) => (dispatch) => {
    const formData = new FormData();
    formData.append('Comments', message );
    formData.append('Image', file );
    httpPost(orderDataUrl.replace('%id%', idOrder))
      .then((response) => {
        console.log('response orderDataUrl', response);
        console.log('getDataFromResponse', getDataFromResponse(response));
        dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
        dispatch({ type: LOAD_ORDER_DATA });
      })
      .catch((error) => {
        console.log('error', error.message);
        dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
      });
  },
  showAmendmentsModalAction: (isShow) => (dispatch) => {
    dispatch({ type: SHOW_AMENDMENTS_MODAL, isShow });
  },
};

function loadOrderData(state, action) {
  return {
    ...state,
    ...action.data,
  };
}

function loadingOrderData(state, action) {
  return {
    ...state,
    isLoading: action.isLoading,
  };
}
/* */
function sendAmendments(state, action) {
  return {
    ...state,
  };
}
/** */
function showAmendmentsModal(state, action) {
  return {
    ...state,
    isShowAmendmentsModal: action.isShow,
  };
}

function orderNotFound(state, action) {
  return {
    ...state,
    isOrderNotFound: action.isNotFound,
  };
}

export const reducer = (state = initialStateOrder, action) => {
  const reduceObject = {
    [LOAD_ORDER_DATA]: loadOrderData,
    [LOADING_ORDER_DATA]: loadingOrderData,
    [SEND_AMENDMENTS]: sendAmendments,
    [SHOW_AMENDMENTS_MODAL]: showAmendmentsModal,
    [ORDER_NOT_FOUND]: orderNotFound,
  };

  return reduceObject.hasOwnProperty(action.type) ? reduceObject[action.type](state, action) : state;
}