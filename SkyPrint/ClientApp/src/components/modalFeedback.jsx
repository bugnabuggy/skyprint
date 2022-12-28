import React, { useCallback } from "react";
import Modal from 'react-awesome-modal';

import { OrderPreview } from './orderPreviewComponent';

export function ModalFeedback(props) {
    const {
        order,
        orderId,
        closeFeedback,
        sendAmendments
    } = props;

    const approve = useCallback((e) => {
        e.preventDefault();

        if (e.target.checkValidity()) {
            const formData = new FormData(e.target);
            formData.append('Status', 0);
            sendAmendments(orderId, formData);
        }

        return false;
    }, []);


    return <Modal
        visible

        width="684"
        effect="fadeInDown"
        onClickAway={closeFeedback}
    >
        <form className='modal-flex' onSubmit={approve} >
            <OrderPreview order={order} />
            <div className="modal-feedback">
                <div>
                    <label>Номер макета, утвержаемого в печать</label>
                    <input
                        type="text"
                        name='SelectedFrame'
                        placeholder="номера макетов через запятую"
                    />
                </div>
                <div>
                    <label>Оцените работу дизайнера от 0-10 </label>
                    <input
                        type="number"
                        name='Rating'
                        min={0}
                        max={10}
                        step={1}
                        required />
                </div>
                <div>
                    <label>Хотели бы работать в следующий раз с этим дизайнером</label>
                    <input
                        name='LikeDesigner'
                        value='true'
                        type="checkbox" />
                </div>
                <div>
                    <label>Комментарий</label>
                    <textarea
                        name='Feedback'
                    />
                </div>

                <button className="o-btn-primary o-btn-info-field" type="submit">Отправить</button>


            </div>
        </form>
    </Modal>
}