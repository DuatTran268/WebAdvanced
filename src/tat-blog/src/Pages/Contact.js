import React, { useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faAddressCard,
  faEnvelope,
  faGlobeOceania,
  faInfoCircle,
  faMapLocation,
  faPhone,
  faQuestionCircle,
} from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import { Button } from "react-bootstrap";

const Contact = () => {
  useEffect(() => {
    document.title = "Liên hệ";
  }, []);

  return (
    <div className="container pt-5">
      <div className="row card">
        <div className="card-header">
          <FontAwesomeIcon icon={faInfoCircle} className="text-primary" />
          <span className="text-primary">
            Thông tin liên hệ, hãy liên hệ với chúng tôi
          </span>
        </div>
        <div className="card-body">
          <div className="col">
            <div className="card_item pt-3">
              <FontAwesomeIcon icon={faAddressCard} />
              Địa chỉ:
              <Link
                to={"https://www.google.com/maps"}
                target="_blank"
                className="text-decoration-none"
              >
                95 Lý Nam Đế - Phường 8 - Thành phố Đà Lạt - Lâm Đồng
              </Link>
            </div>

            <div className="card_item pt-3">
              <FontAwesomeIcon icon={faEnvelope} />
              Email:
              <Link
                to={"mailto:2012254@dlu.edu.vn"}
                className="text-decoration-none"
              >
                2012254@dlu.edu.vn
              </Link>
            </div>

            <div className="card_item pt-3">
              <FontAwesomeIcon icon={faPhone} />
              Phone:
              <Link to={"tel:0966668888"} className="text-decoration-none">
                0966668888
              </Link>
            </div>

            <div className="card_item pt-3">
              <FontAwesomeIcon icon={faGlobeOceania} />
              Facebook:
              <Link
                to={"https://www.facebook.com/tran.duat.2368"}
                target="_blank"
                className="text-decoration-none"
              >
                Duật Trần
              </Link>
            </div>
          </div>
        </div>
      </div>

      <div className="row card mt-3">
        <div className="row">
          <div className="col-6">
            <div className="card-header">
              <FontAwesomeIcon icon={faQuestionCircle} />
              <span className="text-success">Gửi ý kiến của bạn tại đây</span>
            </div>

            <div className="card-body">
              <form action="" method="post">
                <div className="form-group">
                  <label>Tên: </label>
                  <input
                    type="text"
                    className="form-control"
                    placeholder="Nhập tên của bạn" required
                  ></input>
                </div>
                <div className="form-group">
                  <label>Địa chỉ Email: </label>
                  <input
                    type="email"
                    className="form-control"
                    placeholder="Nhập Email của bạn"
                    required
                  ></input>
                </div>
                <div className="form-group">
                  <label>Nội dung: </label>
                  <textarea
                    type="text"
                    rows={6}
                    className="form-control"
                    placeholder="Nhập nội dung"
                    required
                  ></textarea>
                </div>
                <br />
                <div className="justify-content-center d-flex">
                  <Button type="submit" className="btn btn-primary">
                    Gửi phản hồi
                  </Button>
                </div>
              </form>
            </div>
          </div>

          <div class="col-6">
              <div class="card-header">
                <FontAwesomeIcon icon={faMapLocation} />
                <span class="text-success">Bản đồ</span>
              </div>
              <div class="card-body">
              <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3903.2877902405253!2d108.44201621412589!3d11.95456563961217!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x317112d959f88991%3A0x9c66baf1767356fa!2zVHLGsOG7nW5nIMSQ4bqhaSBI4buNYyDEkMOgIEzhuqF0!5e0!3m2!1svi!2s!4v1633261535076!5m2!1svi!2s" width="100%" height="360px" frameborder="0" style={{border:0}} allowfullscreen="" aria-hidden="false" tabindex="0"></iframe>
              </div>
            </div>
        </div>
      </div>
    </div>
  );
};

export default Contact;
