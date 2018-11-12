import { httpGet } from '../service/httpService';
import { orderDataUrl } from '../service/api';

const LOAD_ORDER_DATA = 'LOAD_ORDER_DATA';
const LOADING_ORDER_DATA = 'LOADING_ORDER_DATA';
const SEND_AMENDMENTS = 'SEND_AMENDMENTS';
const SHOW_AMENDMENTS_MODAL = 'SHOW_AMENDMENTS_MODAL';
export const initialStateOrder = {
  name: 'gsdfgsdfgsdfg',
  picture: 'https://upload.wikimedia.org/wikipedia/commons/c/cc/ESC_large_ISS022_ISS022-E-11387-edit_01.JPG',
  info: '',
  address: '',
  hasClientAnswer: false,
  status: '',
  isLoading: false,
  isShowAmendmentsModal: false,
};

export const actionCreators = {
  loadDataAction: (idOrder) => (dispatch, getState) => {
    dispatch({ type: LOADING_ORDER_DATA, isLoading: true });
    httpGet(orderDataUrl.replace('%id%', idOrder))
      .then((response) => {
        console.log('response orderDataUrl', response);
        dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
        dispatch({ type: LOAD_ORDER_DATA });
      })
      .catch((error) => {
        console.log('error', error.message);
            dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
      });
  },
  sendAmendmentsAction: (message, file) => (dispatch) => {

  },
  showAmendmentsModalAction: (isShow) => (dispatch) => {
    dispatch({type: SHOW_AMENDMENTS_MODAL, isShow});
  }
};

function loadOrderData(state, action) {
  return {
    ...state,
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

export const reducer = (state = initialStateOrder, action) => {
  const reduceObject = {
    [LOAD_ORDER_DATA]: loadOrderData,
    [LOADING_ORDER_DATA]: loadingOrderData,
    [SEND_AMENDMENTS]: sendAmendments,
    [SHOW_AMENDMENTS_MODAL]: showAmendmentsModal,
  };

  return reduceObject.hasOwnProperty(action.type) ? reduceObject[action.type](state, action) : state;
}
