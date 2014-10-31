PrintInt PROC
    push rbp
	push R15
	push R14
	push R13
	push R12
	push R9
	push R8
    mov rbp, rsp

    MOV RDX, RCX    

    .data
        strPrintIntFormat db "%d",0

    .code
    LEA RCX, strPrintIntFormat
	sub rsp, 40
    call printf    
	add rsp, 40   

    mov rsp, rbp
	pop R8
	pop R9
	pop R12
	pop R13
	pop R14
	pop R15
    pop rbp
    ret
PrintInt ENDP

PrintDouble PROC
    push rbp
	push R15
	push R14
	push R13
	push R12
	push R9
	push R8
    mov rbp, rsp

	MOVD RDX, XMM0

    .data
        strPrintDoubleFormat db "%f",0

    .code
    LEA RCX, strPrintDoubleFormat
	sub rsp, 40
    call printf    
	add rsp, 40  

    mov rsp, rbp
	pop R8
	pop R9
	pop R12
	pop R13
	pop R14
	pop R15
    pop rbp
    ret
PrintDouble ENDP

PrintLine PROC
    push rbp
	push R15
	push R14
	push R13
	push R12
	push R9
	push R8
    mov rbp, rsp

    MOV RDX, RCX    

    .data
        strPrintLineFormat db "%s",10,0

    .code
    LEA RCX, strPrintLineFormat

	sub rsp, 40
    call printf    
	add rsp, 40 

    mov rsp, rbp
	pop R8
	pop R9
	pop R12
	pop R13
	pop R14
	pop R15
    pop rbp
    ret
PrintLine ENDP

Power PROC
    push rbp
	push R15
	push R14
	push R13
	push R12
	push R9
	push R8
    mov rbp, rsp

	sub rsp, 40
    call pow    
	add rsp, 40

    mov rsp, rbp
	pop R8
	pop R9
	pop R12
	pop R13
	pop R14
	pop R15
    pop rbp
    ret
Power ENDP

Alloc PROC
    push rbp
	push R15
	push R14
	push R13
	push R12
	push R9
	push R8
    mov rbp, rsp
	
	sub rsp, 40
    call malloc    
	add rsp, 40

    mov rsp, rbp
	pop R8
	pop R9
	pop R12
	pop R13
	pop R14
	pop R15
    pop rbp
    ret
Alloc ENDP