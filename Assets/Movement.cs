using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private core.Processor proc;

	// Use this for initialization
	void Start () {
        proc = new core.Processor();
        var script = @"
LOOP    LDA 0x10
        CMA 0x01
        JLT LOOP
        JEQ UP
        CMA 0x02
        JEQ RIGHT
        CMA 0x03
        JEQ DOWN
        CMA 0x04
        JEQ LEFT
        JMP LOOP
UP      LDA 0x01
        STA 0x21
        JMP LOOP
RIGHT   LDA 0x02
        STA 0x20
        JMP LOOP
DOWN    LDA 0x02
        STA 0x21
        JMP LOOP
LEFT    LDA 0x01
        STA 0x20
        JMP LOOP";
        var asm = new assembler.Assembler();
        var prog = asm.Assemble(script, 1000);
        proc.Load(prog, 1000);
        proc.PC = 1000;
	}
	
	// Update is called once per frame
	void Update () {
        bool sawx = false;
        var xin = Input.GetAxis("Horizontal");
        if (xin == 0f) {
            proc.memory[0x10] = 0;
        } else if (xin > 0) {
            proc.memory[0x10] = 2;
            sawx = true;
        } else {
            proc.memory[0x10] = 4;
            sawx = true;
        }

        if (!sawx) {
            var yin = Input.GetAxis("Vertical");
            if (yin == 0f) {
                proc.memory[0x10] = 0;
            } else if (yin > 0) {
                proc.memory[0x10] = 3;
            } else {
                proc.memory[0x10] = 1;
            }
        }

        proc.Step();

        var dxi = 0f;
        var procval = proc.memory.Outputs[0x20];
        if (procval != 0) {
            Debug.Log($"Procval: {procval}");
        }
        if (procval == 1) {
            dxi = -1;
        } else if (procval == 2) {
            dxi = 1;
        }
        procval = proc.memory.Outputs[0x21];
        if (procval != 0) {
            Debug.Log($"Procval: {procval}");
        }
        var dyi = 0f;
        if (procval == 1) {
            dyi = -1;
        } else if (procval == 2) {
            dyi = 1;
        }
        dxi = dxi * 0.2f;
        dyi = dyi * 0.2f;
        var dx = new Vector3(dxi, 0f, 0f);
        var dy = new Vector3(0f, dyi, 0f);


        var me = GetComponent<BoxCollider2D>();

        me.enabled = false;

        var body = GetComponent<Rigidbody2D>();

        var coll = Physics2D.OverlapBox(body.position + (Vector2)dx, me.size, 0f);
        if (coll == null) {
            transform.position += dx;
        }
        coll = Physics2D.OverlapBox(body.position + (Vector2)dy, me.size, 0f);
        if (coll == null) {
            transform.position += dy;
        }
        me.enabled = true;
	}
}
