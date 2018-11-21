import { httpGet, httpPost } from '../service/httpService';
import { orderDataUrl } from '../service/api';
import { getDataFromResponse } from '../service/helper';

const LOAD_ORDER_DATA = 'LOAD_ORDER_DATA';
const LOADING_ORDER_DATA = 'LOADING_ORDER_DATA';
const SHOW_AMENDMENTS_MODAL = 'SHOW_AMENDMENTS_MODAL';
const ORDER_NOT_FOUND = 'ORDER_NOT_FOUND';
const SHOW_APPROWED_MODAL = 'SHOW_APPROWED_MODAL';
const SHOW_SPLASH_SCREEN_BANNER = 'SHOW_SPLASH_SCREEN_BANNER';
export const initialStateOrder = {
  name: '',
  picture: '',
  info: '',
  address: '',
  transportCompany: '',
  hasClientAnswer: false,
  status: '',
  fileType: '',
  isLoading: false,
  isShowAmendmentsModal: false,
  isOrderNotFound: false,
  isShowApprovedModal: false,
  isShowBanner: false,
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
        dispatch({ type: ORDER_NOT_FOUND, isNotFound: true });
        dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
      });
  },
  sendAmendmentsAction: (idOrder, data) => (dispatch) => {
    httpPost(orderDataUrl.replace('%id%', idOrder), data)
      .then((response) => {
        const isShow = false;
        dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
        dispatch({ type: LOAD_ORDER_DATA, data: getDataFromResponse(response) });
        dispatch({ type: SHOW_AMENDMENTS_MODAL, isShow });
        dispatch({ type: SHOW_APPROWED_MODAL, isShow });
        dispatch({ type: SHOW_SPLASH_SCREEN_BANNER, isShow: true });
      })
      .catch((error) => {
        const isShow = false;
        dispatch({ type: LOADING_ORDER_DATA, isLoading: false });
        dispatch({ type: SHOW_AMENDMENTS_MODAL, isShow });
        dispatch({ type: SHOW_APPROWED_MODAL, isShow });
      });
  },
  showAmendmentsModalAction: (isShow) => (dispatch) => {
    dispatch({ type: SHOW_AMENDMENTS_MODAL, isShow });
  },
  showApprovedModalAction: (isShow) => (dispatch) => {
    dispatch({ type: SHOW_APPROWED_MODAL, isShow });
  },
  showSplashScreenBannerAction: (isShow) => (dispatch) => {
    dispatch({ type: SHOW_SPLASH_SCREEN_BANNER, isShow });
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

function showApprovedModal(state, action) {
  return {
    ...state,
    isShowApprovedModal: action.isShow,
  };
}

function showSplashScreenBanner(state, action) {
  return {
    ...state,
    isShowBanner: action.isShow,
  };
}

export const reducer = (state = initialStateOrder, action) => {
  const reduceObject = {
    [LOAD_ORDER_DATA]: loadOrderData,
    [LOADING_ORDER_DATA]: loadingOrderData,
    [SHOW_AMENDMENTS_MODAL]: showAmendmentsModal,
    [ORDER_NOT_FOUND]: orderNotFound,
    [SHOW_APPROWED_MODAL]: showApprovedModal,
    [SHOW_SPLASH_SCREEN_BANNER]: showSplashScreenBanner,
  };

  return reduceObject.hasOwnProperty(action.type) ? reduceObject[action.type](state, action) : state;
}
