import React, { useEffect, useState } from "react";
import { isInteger } from "../../../../Utils/Utils";
import { Button } from "react-bootstrap";
import Form from "react-bootstrap/Form";

import {
  getTagById,
  getFilterTag,
  putUpdateTag,
} from "../../../../Services/TagCloudWidget";

import { Link, Navigate, Params, useParams } from "react-router-dom";
import { set } from "date-fns";

const EditTag = () => {
  const [validated, setValidated] = useState(false);
  const initialState = {
      id: 0,
      name: "",
      description: "",
      urlSlug: "",
    },
    [tag, setTag] = useState(initialState);

  let { id } = useParams();
  id = id ?? 0;

  useEffect(() => {
    document.title = "Thêm/cập nhật thẻ ";

    getTagById(id).then((data) => {
      if (data) {
        setTag(data);
      } else {
        setTag(initialState);
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

      putUpdateTag(id, data).then((data) => {
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
        <Form.Control type="hidden" name="id" value={tag.id} />
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Tiêu đề</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="name"
              title="Name"
              required
              value={tag.name || ""}
              onChange={(e) => setTag({ ...tag, name: e.target.value })}
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
              value={tag.urlSlug || ""}
              onChange={(e) => setTag({ ...tag, urlSlug: e.target.value })}
            />
            <Form.Control.Feedback type="invalid">
              Không được bỏ trống
            </Form.Control.Feedback>
          </div>
        </div>

        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">
            Description
          </Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="description"
              title="Description"
              required
              value={tag.description || ""}
              onChange={(e) => setTag({ ...tag, description: e.target.value })}
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

export default EditTag;
