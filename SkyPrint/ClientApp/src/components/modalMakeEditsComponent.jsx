import React from 'react';
import { Button } from 'react-bootstrap';
import Modal from 'react-awesome-modal';

export class MakeEdits extends React.Component {
  handleFile = (e) => {
    console.log(e.target.files[0]);
    this.setState({ file: e.target.files[0] });
  };
  handleText = (e) => {
    this.setState({ text: e.target.value });
  };
  handleSend = () => {
    this.props.sendAmendments(this.props.orderId, this.state.text, this.state.file)
  };
  constructor(props) {
    super(props);
    this.state = {
      text: '',
      file: null,
    };
  }
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
              <textarea
                className="modal-text-area"
                onChange={this.handleText}
              >
              </textarea>
              <span>Загрузите файл</span>
              <input
                type="file"
                onChange={this.handleFile}
              />
            </div>
            <div className="modal-footer-container">
              <Button
                bsStyle="primary"
                onClick={this.handleSend}
                disabled={!this.state.text}
              >
                Отправить
              </Button>
              <Button onClick={this.props.closeModal}>Отменить</Button>
            </div>
        </div>
      </Modal>
    );
  }
}
