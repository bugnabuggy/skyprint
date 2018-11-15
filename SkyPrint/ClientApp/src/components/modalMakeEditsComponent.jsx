import React from 'react';
import Modal from 'react-awesome-modal';

export class MakeEdits extends React.Component {
  handleFile = (e) => {
    this.setState({ file: e.target.files[0] });
  };
  handleText = (e) => {
    this.setState({ text: e.target.value });
  };
  handleSend = () => {
    const formData = new FormData();
    formData.append('Comments', this.state.text);
    formData.append('Image', this.state.file);
    this.props.sendAmendments(this.props.orderId, formData)
  };
  constructor(props) {
    super(props);
    this.state = {
      text: '',
      file: null,
    };
  }
  render() {
    const text = this.state.file ? this.state.file.name : 'Прикрепить файл';
    return (
      <Modal
        visible={this.props.showAmendments}
        width="959"
        height="499"
        effect="fadeInDown"
        onClickAway={this.props.closeModal}
      >
        <div className="modal-window-wishes">
          <div className="fields-container">
            <div className="text-field-container">
              <textarea onChange={this.handleText}></textarea>
            </div>
            <div className="file-fiel-container">
              <img src="/clip.png" className="file-field-image" alt=""/>
              <div className="file-download-link">
                <input type="file" onChange={this.handleFile} alt="Выберете файл" />
                <a>
                  {text}
                </a>
              </div>
            </div>
          </div>
          <div className="fields-container-2">
            <div className="text-field-container-2">
            <img src="/bracket.png" className="backet-image" alt=""/>
              <label>Внесите сюда свои пожелания для дизайнера</label>
            </div>
            <div className="button-field-container">
              <div
                className="button-sent-comments"
                onClick={this.handleSend}
              >
                <span className="o-btn-primary o-btn-info-field">
                  Отправить пожелания
                </span>
              </div>
            </div>
          </div>
        </div>
      </Modal>
    );
  }
}
