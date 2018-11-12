import React from 'react';
import { Button } from 'react-bootstrap';
import Modal from 'react-awesome-modal';

export class MakeEdits extends React.Component {
  handleFile = (e) => {
    console.log(e.target.files[0]);
  };
  render() {
    return (
      <Modal
        visible={this.props.showAmendments}
        width="500"
        height="500"
        effect="fadeInDown"
        onClickAway={this.props.closeModal}
      >
        <div className="modal-container">
          <div className="modal-header-container">
            <h2>Внести правки</h2>
            <div
              className="modal-header-btn-close"
              onClick={this.props.closeModal}
            >
              X
            </div>
          </div>
          <div className="modal-main-container">
            <textarea className="modal-text-area">

            </textarea>
            <input
              type="file"
              onChange={this.handleFile}
              onClick={(e) => {
                console.log(e.target);
              }}
            />
          </div>
          <div className="modal-footer-container">
            <Button bsStyle="primary">Отправить</Button>
            <Button onClick={this.props.closeModal}>Отменить</Button>
          </div>
        </div>
      </Modal>
    );
  }
}
