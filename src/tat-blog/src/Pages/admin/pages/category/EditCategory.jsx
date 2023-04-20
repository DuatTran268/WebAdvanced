import { useEffect, useState } from "react";

import { Link, Navigate, useParams } from "react-router-dom";
import {
  getCategoryById,
  updateCategory,
} from "../../../../Services/CategoriesWidgets";
import { isInteger } from "../../../../Utils/Utils";
import { Form } from "react-router-dom";
import { Button } from "react-bootstrap";

const EditCategory = () => {
  const [validated, setValidated] = useState(false);

  const initialState = {
      id: 0,
      name: "",
      urlSlug: "",
      description: "",
    },
    [category, setCategory] = useState(initialState);

  let { id } = useParams();
  id = id ?? 0;

  useEffect(() => {
    document.title = "Thêm cập nhật categrory";

    getCategoryById(id).then((data) => {
      if (data) {
        setCategory(data);
      } else {
        setCategory(initialState);
      }
    });
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (e.currentTarget.checkValidity() === false) {
      e.stopPropagation();
      setValidated(true);
    } else {
      updateCategory().then((data) => {
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
        <Form.Group type="hidden" name="id" value={category.id}>
          <div className="row mb-3">
            <Form.Label className="col-sm-2 col-form-label">
              Tên category
            </Form.Label>
            <div className="col-sm-10">
              <Form.Control
                type="text"
                name="name"
                title="Name"
                required
                value={category.name || ""}
                onChange={(e) =>
                  setCategory({...category, name: e.target.value })
                }
              />

              <Form.Control type="invalid">
                Không được bỏ trống.
              </Form.Control>
            </div>
          </div>

          <div className="text-center">
            <Button variant="primary" type="submit">
              Lưu các thay đổi
            </Button>
            <Link to="/admin/categories" className="btn btn-danger ms-2">
              Hủy và quay lại
            </Link>
          </div>
        </Form.Group>
      </Form>
    </>
  );
};

export default EditCategory;
