import React, { useEffect, useState } from "react";
import { Button, Form } from "react-bootstrap";
import { Link, Navigate, useParams } from "react-router-dom";
import {
  getAuthorById,
  updateAuthor,
} from "../../../../Services/TopAuthorWidget";
import { isEmptyOrSpaces, isInteger } from "../../../../Utils/Utils";

const EditAuthor = () => {
  const [validated, setValidated] = useState(false);
  const initialState = {
      id: 0,
      imageUrl: "",
      fullName: "",
      email: "",
    },
    [author, setAuthor] = useState(initialState);

  let { id } = useParams();
  id = id ?? 0;

  useEffect(() => {
    document.title = "Thêm cập nhật tác giả";

    getAuthorById(id).then((data) => {
      if (data) {
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
      updateAuthor().then((data) => {
        if (data) {
          alert("Đã lưu thành công");
        } else {
          alert("Xảy ra lỗi khi lưu");
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
        <Form.Group type="hidden" name="id" value={author.id}>
          <div className="row mb-3">
            <Form.Label className="col-sm-2 col-form-label">
              Fullname
            </Form.Label>
            <div className="col-sm-10">
              <Form.Control
                type="text"
                name="name"
                title="Name"
                required
                value={author.fullName || ""}
                onChange={(e) => setAuthor({ ...author, name: e.target.value })}
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
                onChange={(e) =>
                  setAuthor({ ...author, urlSlug: e.target.value })
                }
              />
              <Form.Control.Feedback type="invalid">
                Không được bỏ trống
              </Form.Control.Feedback>
            </div>
          </div>

          <div className="row mb-3">
            <Form.Label className="col-sm-2 col-form-label">Email</Form.Label>
            <div className="col-sm-10">
              <Form.Control
                type="text"
                name="email"
                title="Url Slug"
                required
                value={author.email || ""}
                onChange={(e) =>
                  setAuthor({ ...author, email: e.target.value })
                }
              />
              <Form.Control.Feedback type="invalid">
                Không được bỏ trống
              </Form.Control.Feedback>
            </div>
          </div>

          {!isEmptyOrSpaces(author.imageUrl) && (
            <div className="row mb-3">
              <Form.Label className="col-sm-2 col-form-label">
                Hình hiện tại
              </Form.Label>
              <div className="col-sm-10">
                <img src={author.imageUrl} alt={author.title} />
              </div>
            </div>
          )}

          <div className="row mb-3">
            <Form.Label className="col-sm-2 col-form-label">
              Chọn hình ảnh
            </Form.Label>
            <div className="col-sm-10">
              <Form.Control
                type="file"
                name="imageFile"
                accept="image/*"
                title="Image file"
                onChange={(e) =>
                  setAuthor({ ...author, imageFile: e.target.files[0] })
                }
              />
              <Form.Control.Feedback type="invalid">
                Không được bỏ trống.
              </Form.Control.Feedback>
            </div>
          </div>

          <div className="text-center">
            <Button variant="primary" type="submit">
              Lưu các thay đổi
            </Button>
            <Link to="/admin/authors" className="btn btn-danger ms-2">
              Hủy và quay lại
            </Link>
          </div>
        </Form.Group>
      </Form>
    </>
  );
};

export default EditAuthor;
