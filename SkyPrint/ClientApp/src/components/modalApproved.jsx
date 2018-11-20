import React from 'react';
import Modal from 'react-awesome-modal';

export class ModalApproved extends React.Component {
  handleApprove = (event) => {
    const formData = new FormData();
    formData.append('Status', 0);
      this.props.sendAmendments(this.props.orderId, formData);
  };
  render() {
    return (
      <Modal
        visible={this.props.show}
        width="684"
        height="365"
        effect="fadeInDown"
        onClickAway={this.props.closeApprove}
      >
        <div className="modal-approve-text">
          <h3><label>
            <p>Я ВНИМАТЕЛЬНО проверил весь текст,</p>
            <p>адреса, телефоны, цены. Ошибок нет.</p>
            <p>ПОЛНОСТЬЮ НЕСУ ЗА ЭТО ОТВЕТСТВЕННОСТЬ.</p>
            <p>Можете отдавать в печать.</p>
          </label></h3>
        </div>
        <div className="modal-approve-buttons">
          <div
          className="approve-button approve-button-yes"
            onClick={this.handleApprove}
          >
            <span className="o-btn-primary o-btn-info-field modal-approve-button">
              Да
            </span>
          </div>
          <div
          className="approve-button approve-button-cancel"
            onClick={this.props.closeApprove}
          >
            <span className="o-btn-primary o-btn-info-field modal-approve-button">
              Проверю еще разок
            </span>
          </div>
        </div>
      </Modal>
    );
  }
}