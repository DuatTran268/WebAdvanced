/* eslint-disable react-hooks/exhaustive-deps */
import React, { useEffect, useState } from "react";
import { Button } from "react-bootstrap";
import Form from "react-bootstrap/Form";
import { Link, Navigate, useParams } from "react-router-dom";
import { getAuthorById, updateAuthor } from "../../../../Services/TopAuthorWidget";
import { isInteger } from "../../../../Utils/Utils";

const EditAuthor = () => {
  const [validated, setValidated] = useState(false);
  const initialState = {
      id: 0,
      name: "",
      urlSlug: "",
    },
    [author, setAuthor] = useState(initialState);

  let { id } = useParams();
  id = id ?? 0;

  useEffect(() => {
    document.title = "Thêm/cập nhật thẻ ";

      getAuthorById(id).then((data) => {
      if (data) {
        console.log("data: ", data);
        setAuthor(data);
      } else {
        setAuthor(initialState);
      }
    });
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (e.currentTarget.checkValidity() === false) {
      e.stopPropagation();
      setValidated(true);
    } else {
      let data = new FormData(e.target);

      updateAuthor(id, data).then((data) => {
        if (data) {
          alert("Đã lưu thành công");
        } else {
          alert("Đã xảy ra lỗi khi lưu");
        }
      });
    }
  };

  if (id && !isInteger(id))
    return <Navigate to={`/400?redirectTo=/admin/posts`} />;

  return (
    <>
      <Form
        method="post"
        encType=""
        onSubmit={handleSubmit}
        noValidate
        validated={validated}
      >
        <Form.Control type="hidden" name="id" value={author.id} />
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Full Name</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="fullName"
              title="FullName"
              required
              value={author.fullName || ""}
              onChange={(e) => setAuthor({ ...author, fullName: e.target.value })}
            />
            <Form.Control.Feedback type="invalid">
              Không được bỏ trống.
            </Form.Control.Feedback>
          </div>
        </div>

        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">UrlSlug</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="urlSlug"
              title="Url Slug"
              required
              value={author.urlSlug || ""}
              onChange={(e) => setAuthor({ ...author, urlSlug: e.target.value })}
            />
            <Form.Control.Feedback type="invalid">
              Không được bỏ trống
            </Form.Control.Feedback>
          </div>
        </div>

        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">
            Email
          </Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="email"
              title="Email"
              required
              value={author.email || ""}
              onChange={(e) => setAuthor({ ...author, description: e.target.value })}
            />
            <Form.Control.Feedback type="invalid">
              Không được bỏ trống
            </Form.Control.Feedback>
          </div>
        </div>

        <div className="text-center">
          <Button variant="primary" type="submit">
            Lưu các thay đổi
          </Button>
          <Link to="/admin/tags" className="btn btn-danger ms-2">
            Hủy và quay lại
          </Link>
        </div>
      </Form>
    </>
  );
};

export default EditAuthor;
