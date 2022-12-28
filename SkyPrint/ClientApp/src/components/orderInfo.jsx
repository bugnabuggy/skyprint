import React from 'react';

import { InfoField } from './infoFieldComponent';
import { MakeEdits } from './modalMakeEditsComponent';
import { ArrowComponent } from './arrowComponent';
import { ModalApproved } from './modalApproved';
import { ModalFeedback } from './modalFeedback';

export function OrderInfo(props) {
  const {
    order,
    orderId,
    sendAmendments,
    showAmendmentsModalAction,
    showApprovedModalAction,
    showFeedbackModalAction,
    showSplashScreenBannerAction,
    closeModal,
    closeApprove,
    closeFeedback
  } = props;

  const editCmpnt = order.isShowAmendmentsModal
    ? <MakeEdits
      order={order}
      orderId={orderId}
      showAmendments={order.isShowAmendmentsModal}
      closeModal={closeModal}
      sendAmendments={sendAmendments}
    />
    : null;

  const modalCmpnt = order.isShowApprovedModal
    ? <ModalApproved
      show={order.isShowApprovedModal}
      orderId={orderId}
      showFeedbackModalAction={showFeedbackModalAction}
      closeApprove={closeApprove}
    />
    : null;

  const feedbackCmpnt = order.isShowFeedback
    ? <ModalFeedback
      order={order}
      orderId={orderId}
      closeFeedback={closeFeedback}
      sendAmendments={sendAmendments}
    />
    : null;

  return (
    <React.Fragment>
      <div>
        <InfoField
          order={order}
          orderId={orderId}
          sendAmendments={sendAmendments}
          showAmendmentsModalAction={showAmendmentsModalAction}
          showApprovedModalAction={showApprovedModalAction}
          showSplashScreenBannerAction={showSplashScreenBannerAction}
        />
        {editCmpnt}
        {modalCmpnt}
        {feedbackCmpnt}
      </div>
      <ArrowComponent />
    </React.Fragment>
  )
}
