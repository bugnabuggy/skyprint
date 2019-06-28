import React from 'react';
import Modal from 'react-awesome-modal';
import { FileComponent } from './fileComponent'

export class MakeEdits extends React.Component {
    handleFile = (e, index) => {
        if (this.state.files[index] == undefined) {
            this.state.files.push(undefined);
        }
        if (e.target.files[0] == undefined) {
            this.state.files.splice(index, 1)
        } else {
            this.state.files[index] = e.target.files[0]
        }
        
        this.setState({ files: this.state.files});
    };
    handleText = (e) => {
        this.setState({ text: e.target.value });
    };
    handleSend = () => {
        this.state.files = this.state.files.filter( x => x != undefined);
        const formData = new FormData();
        formData.append('Status', 1);
        formData.append('Comments', this.state.text);
        this.state.files.forEach(x=>{
            formData.append('Images[]', x);
        })
        
        this.props.sendAmendments(this.props.orderId, formData);
    };

    handleDeleteFile = ( index) => {
        if (this.state.files[index] != undefined) {
            this.state.files.splice(index, 1);
            this.setState({ files: this.state.files});
        }
    }

    constructor(props) {
        super(props);
        this.state = {
            text: '',
            files: [],
        };
    }
    render() {
        var fileComponents =  [];
        if (this.state.files.length > 0) {
            fileComponents = this.state.files.map( (x, index) => {
                if (x == null) {
                    return <FileComponent index={index} key={index} file={undefined}
                    handleDeleteFile={(index) => this.handleDeleteFile(index)}
                     handleFile={(file, index) => this.handleFile(file, index)} />
                }
                return <FileComponent index={index} key={index} file={x} 
                handleDeleteFile={(index) => this.handleDeleteFile(index)}
                handleFile={(file, index) => this.handleFile(file, index)} />
         });
        } else {
            this.state.files.push(undefined);
            fileComponents.push(<FileComponent index={0}  key={0} file={ undefined} 
                handleDeleteFile={(index) => this.handleDeleteFile(index)}
                handleFile={(file, index) => this.handleFile(file, index)} />)
        }
        
        return (
            <Modal
                visible={this.props.showAmendments}
                width="959"
                effect="fadeInDown"
                portalClassName= "modal-flex"
                onClickAway={this.props.closeModal}
            >
                <div className="modal-window-wishes">
                    <div className="fields-container">
                        <div className="text-field-container">
                            <textarea onChange={this.handleText}></textarea>
                        </div>                        
                        {fileComponents}
                    </div>
                    <div className="fields-container-2">
                        <div className="text-field-container-2">
                            <img src="/bracket.png" className="backet-image" alt="" />
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
