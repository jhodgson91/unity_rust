#[repr(C)]
pub struct Vector2 {
    pub x: i32,
    pub y: i32,
}

#[repr(C)]
pub struct Vector3 {
    pub x: f32,
    pub y: f32,
    pub z: f32,
}

#[repr(C)]
pub struct Quaternion {
    pub x: f32,
    pub y: f32,
    pub z: f32,
    pub w: f32,
}

#[no_mangle]
pub extern "C" fn test_ref_value_array(data: *mut Vector3, len: usize) {
    unsafe {
        let slice = std::slice::from_raw_parts_mut(data, len);
        slice[4].x = len as f32;
    }
}

#[no_mangle]
pub extern "C" fn test_return_vec2() -> Vector2 {
    Vector2 { x: 1, y: 1 }
}

#[no_mangle]
pub extern "C" fn test_ref_vec3(v: &mut Vector3) {
    v.x += 1.0
}
